using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PresentationTraker.Models;
using PresentationTraker.Models.Server;
using PresentationTraker.View;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace PresentationTraker.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userFirstName;
        [ObservableProperty]
        private string userLastName;
        [ObservableProperty]
        private string userEmail;
        [ObservableProperty]
        private string userPassword;
        [ObservableProperty]
        public Visibility anuntificatedFormVisibility = Visibility.Visible;
        [ObservableProperty]
        public SolidColorBrush borderBrushLogInButton = new SolidColorBrush(Colors.Black);
        [ObservableProperty]
        public Page currentPage;
        private string token;
        private Page homePg;
        private Page profilePg;
        private readonly ApiImplementation _api;
        
        public IAsyncRelayCommand LogInCommand { get; }
        public MainViewModel()
        {
            UserFirstName = "first name";
            UserLastName = "last name";
            UserEmail = "email";
            userPassword = "password";
            _api = new ApiImplementation(EndPoints.BaseUrl);
            LogInCommand = new AsyncRelayCommand(ExecuteAuthenticate);

            
        }
        public async Task ExecuteAuthenticate() {
            string? responseContent = string.Empty;
            try
            {
                RestResponse response = await _api.Authenticate(UserEmail, UserPassword, UserLastName, UserFirstName);
                if (response.ResponseStatus  == ResponseStatus.Completed)
                {
                    responseContent = JsonSerializer.Deserialize<string>(response.Content);
                    if (responseContent is not null)
                    {
                        token = responseContent;
                        CurrentPage = homePg = new View.HomeWindow(token);
                        AnuntificatedFormVisibility = Visibility.Hidden;
                    }
                }
                BorderBrushLogInButton = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                throw new Exception();
                
            }
            await WriteToken(token);
        }
        public async Task WriteToken(string data) {
            using (StreamWriter writer = new StreamWriter("Token.txt",false))
            {
               await writer.WriteLineAsync(data);
            }
        }
       
    }
}
