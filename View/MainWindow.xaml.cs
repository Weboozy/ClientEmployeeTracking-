using PresentationTraker.ViewModel;
using System.Text;
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

namespace PresentationTraker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
;
            }
        }

        

        private void UserPasswordField_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length == 0)
            {
                switch (textBox.Name)
                {
                    case "UserFirstNameField":
                        textBox.Text = "first name";
                        break;
                    case "UserLastNameField":
                        textBox.Text = "last name";
                        break;
                    case "UserEmailField":
                        textBox.Text = "email";
                        break;
                    case "UserPasswordField":
                        textBox.Text = "password";
                        break;
                    default:
                        break;
                }
            }
        }

        private void UserPasswordField_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
                switch (textBox.Name)
                {
                    case "UserFirstNameField":
                        if (textBox.Text == "first name")
                            textBox.Text = "";
                        break;
                    case "UserLastNameField":
                        if (textBox.Text == "last name")
                            textBox.Text = "";
                        break;
                    case "UserEmailField":
                        if (textBox.Text == "email")
                            textBox.Text = "";
                        break;
                    case "UserPasswordField":
                        if (textBox.Text == "password")
                            textBox.Text = "";
                        break;
                    default:
                        break;
                }
        }
    }
}