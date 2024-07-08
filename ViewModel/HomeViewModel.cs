using PresentationTraker.Models.Server;
using RestSharp;
using PresentationTraker.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using OpenTK.Graphics.GL;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Threading;
using System.Windows.Navigation;
using Microsoft.Xaml.Behaviors.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Markup;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace PresentationTraker.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {

        [ObservableProperty]
        public string activityTime;
        [ObservableProperty]
        public int amountSecondsActivityChart ;
        [ObservableProperty]
        public ChartValues<ObservableValue> activityChart;
        [ObservableProperty]
        public string productiveTime;
        [ObservableProperty]
        public int amountSecondsProductiveChart;
        [ObservableProperty]
        public ChartValues<ObservableValue> productiveChatr;
        [ObservableProperty]
        public string selectedDate;
        [ObservableProperty]
        public Profile selectedProfile;
        [ObservableProperty]
        public SolidColorBrush colorSelectedPerson;
        [ObservableProperty]
        public Visibility stateVisitbilityPersonDetails;

        private string? trackingProfile;

        private readonly ApiImplementation _api;
        public ObservableCollection<Program> ListPrograms { get; set; } = new ObservableCollection<Program>();
        public ObservableCollection<Profile> ListProfiles { get; set; } = new ObservableCollection<Profile>();


        public List<string> ListTrackingPrograms { get; set; } = new List<string> {
            "Microsoft Visual Studio",
            "Microsoft Visual Studio Code",
            "pgAdmin"
        };
      

        public IAsyncRelayCommand SelectProgramCommand { get; }
        public IAsyncRelayCommand SelectEmployeeCommand { get; }
        public IAsyncRelayCommand AddDayToDateCommand { get; } 
        public IAsyncRelayCommand SubtractDayToDateCommand { get; }
        public RelayCommand ManageProfileDetailsPanelCommand { get; }

        private DispatcherTimer uiDispatcher;

        public HomeViewModel(string token)
        {
            StateVisitbilityPersonDetails = Visibility.Hidden;
            _api = new ApiImplementation(EndPoints.BaseUrl,token);
            SelectedDate = "Today";
            trackingProfile = null;

            SelectProgramCommand = new AsyncRelayCommand<object>(SelectProgramCommandHandler);
            SelectEmployeeCommand = new AsyncRelayCommand<object>(SelectEmployeeCommandHandler);
            AddDayToDateCommand = new AsyncRelayCommand(AddDayToDateCommandHandler);
            SubtractDayToDateCommand = new AsyncRelayCommand(SubtractDayToDateCommandHandler);
            ManageProfileDetailsPanelCommand = new RelayCommand(ManageProfileDetailsPanelCommandHandler);

            uiDispatcher = new DispatcherTimer();
            uiDispatcher.Interval = TimeSpan.FromMinutes(1);
            uiDispatcher.Tick += async (sender, e) => { await TrackingUpdates(SelectedDate); };
            uiDispatcher.Start();
        }
        public void ManageProfileDetailsPanelCommandHandler() { 
            if (StateVisitbilityPersonDetails == Visibility.Hidden) {
                StateVisitbilityPersonDetails = Visibility.Visible;
            }
            else
            {
                StateVisitbilityPersonDetails = Visibility.Hidden;
            }
        }
        public async Task SelectEmployeeCommandHandler(object source) {
            //Profile selectedEmployeeProfile = (Profile)source;
            //RestResponse response = await _api.GetAllActivity();
            //string trackingDate = SelectedDate == "Today" ? DateOnly.FromDateTime(DateTime.Now).ToString() : SelectedDate;
            //List<ProfileActivity> activity = JsonConvert.DeserializeObject<List<ProfileActivity>>(response.Content);
            //ProfileActivity? selectedActivity = activity.FirstOrDefault(x => x.profileId == selectedEmployeeProfile.DeviceId
            //                                                            &&x.createdAt.ToString().Contains(trackingDate));

            //SelectProgramsByEmployee(selectedActivity.id.ToString());
        }
        public async Task AddDayToDateCommandHandler() {
            if (SelectedDate != "Today")
            {
                SelectedDate = (DateOnly.Parse(SelectedDate).AddDays(1)).ToString();
                if (SelectedDate == DateOnly.FromDateTime(DateTime.Now).ToString())
                {
                    SelectedDate = "Today";
                    await GetAllPrograms(DateOnly.FromDateTime(DateTime.Now).ToString());
                }
                else
                {
                    await GetAllPrograms(SelectedDate);
                }
            }

        }
        public async Task SubtractDayToDateCommandHandler()
        {
            if (SelectedDate == "Today")
            {
                SelectedDate = (DateOnly.FromDateTime(DateTime.Now.AddDays(-1))).ToString() ;
            }
            else
            {
                if (DateOnly.Parse(SelectedDate) > DateOnly.FromDateTime(DateTime.Now).AddDays(-7))
                {
                    SelectedDate = (DateOnly.Parse(SelectedDate).AddDays(-1)).ToString();
                }
                
            }
            await GetAllPrograms(SelectedDate);
        }
        public async Task SelectProgramCommandHandler(object source) {
            Program program = (Program)source;
            ColorSelectedPerson = program.StatusProgram;
            RestResponse resoponse = await _api.GetActivityById(program.ActivityId.ToString());
            ProfileActivity activity = JsonConvert.DeserializeObject<ProfileActivity>(resoponse.Content);
            RestResponse profile = await _api.GetProfileById(activity.profileId.ToString());
            SelectedProfile = JsonConvert.DeserializeObject<Profile>(profile.Content);
            StateVisitbilityPersonDetails = Visibility.Visible;
        }
        public async Task GetAllPrograms(string date)
        {
            try
            {
                RestResponse response = await _api.GetAllPrograms();
                
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    List<Program> data = JsonConvert.DeserializeObject<List<Program>>(response.Content);
                    List<Program> dataSortedByDate = new List<Program>();
                    foreach (var program in data)
                    {
                        if (program.CreatedAt.ToString().Contains(date))
                        {
                            foreach (var item in ListTrackingPrograms)
                            {
                                if (program.Name.Contains(item))
                                {
                                    program.AllowedProgram = true;
                                    program.StatusProgram = new SolidColorBrush(Color.FromRgb(131, 221, 234));
                                    break;
                                }
                                else
                                {
                                    program.StatusProgram = new SolidColorBrush(Color.FromRgb(243, 124, 109));
                                }
                            }
                            dataSortedByDate.Add(program);
                        }
                    }
                    FillListPrograms(dataSortedByDate);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void FillListPrograms(List<Program> source)
        {
            ListPrograms.Clear();
            int i = 0;
            int removePoint = 0;
            int[] counterActivityTime = new int[3];
            int[] counterProductiveTime = new int[3];
            foreach (var program in source)
            {
                program.Duration = (DateTime.Parse(program.ExitTime) - DateTime.Parse(program.EntryTime)).ToString();
                removePoint = program.EntryTime.IndexOf('.');
                program.Duration = program.Duration.Remove(removePoint, program.Duration.Length - removePoint);
                removePoint =program.EntryTime.IndexOf('.');
                program.EntryTime = program.EntryTime.Remove(removePoint, program.EntryTime.Length - removePoint);
                removePoint = program.ExitTime.IndexOf('.');
                program.ExitTime = program.ExitTime.Remove(removePoint, program.ExitTime.Length - removePoint);
                removePoint = program.CreatedAt.ToString().IndexOf(' ');
                program.CreatedAt =DateTime.Parse(program.CreatedAt.ToShortDateString());
                i++;
                program.Number = i;
                ListPrograms.Add(program);
                counterActivityTime = CalculatingWorkTime(program.Duration,counterActivityTime);
                if (program.AllowedProgram)
                {
                    counterProductiveTime = CalculatingWorkTime(program.Duration, counterProductiveTime);
                }
            }
            ActivityChart = new ChartValues<ObservableValue> { new ObservableValue((counterActivityTime[0] * 3600) + (counterActivityTime[1] * 60) + counterActivityTime[2]) };
            ProductiveChatr = new ChartValues<ObservableValue> { new ObservableValue((counterProductiveTime[0] * 3600) + (counterProductiveTime[1] * 60) + counterProductiveTime[2]) };
            ActivityTime = counterActivityTime[0].ToString() + "h "
                           + counterActivityTime[1].ToString() + "m "
                           + counterActivityTime[2].ToString() + "s";
            ProductiveTime = counterProductiveTime[0].ToString() + "h "
                           + counterProductiveTime[1].ToString() + "m "
                           + counterProductiveTime[2].ToString() + "s";
        }
        private async Task TrackingUpdates(string date)
        {
            DateOnly trackingDate;
            if (date == "Today")
            {
                trackingDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else trackingDate = DateOnly.Parse(date);

            await GetAllPrograms(trackingDate.ToString());
        }
        private int[] CalculatingWorkTime(string time, int[] counterActivityTime ) {
            string[] data = time.Split(':');
            for (int i = data.Length-1;i>0; i--)
            {
                if (data[i] != "00")
                {
                    counterActivityTime[i] += int.Parse(data[i]);
                    if (counterActivityTime[i] > 60)
                    {
                        while (counterActivityTime[i] > 60)
                        {
                            counterActivityTime[i] = counterActivityTime[i] - 60;
                            counterActivityTime[i - 1] = counterActivityTime[i - 1] + 1;
                        }
                    }
                    
                }
            }
            return counterActivityTime;
        }
        public async Task GetAllEmployees() {
            RestResponse response = await _api.GetAllProfiles();
            List<Profile> data = JsonConvert.DeserializeObject<List<Profile>>(response.Content);
            foreach (var profile in data)
            {
                ListProfiles.Add(profile);
            }
        }

        
    }
}
