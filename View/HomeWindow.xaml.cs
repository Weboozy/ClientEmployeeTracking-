using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using PresentationTraker.ViewModel;
using ScottPlot.Rendering.RenderActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace PresentationTraker.View
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Page
    {
        private bool closeListEmployeePanel = false;
        private readonly HomeViewModel _viewModel;
        public SeriesCollection SeriesCollection { get; set; }
        public HomeWindow(string token)
        {
            InitializeComponent();
            _viewModel = new HomeViewModel(token);
            DataContext = _viewModel;
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!closeListEmployeePanel)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 30;
                animation.To = 285;
                animation.Duration = TimeSpan.FromSeconds(0.3);
                EmployeeListPanel.BeginAnimation(HeightProperty, animation);
                closeListEmployeePanel = true;

            }
        }
        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (closeListEmployeePanel)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 285;
                animation.To = 30;
                animation.Duration = TimeSpan.FromSeconds(0.3);
                EmployeeListPanel.BeginAnimation(HeightProperty, animation);
                closeListEmployeePanel = false;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.WhenAll(_viewModel.GetAllPrograms(DateOnly.FromDateTime(DateTime.Now).ToString()), _viewModel.GetAllEmployees());
        }
    }
}
