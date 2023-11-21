using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeeXnes.Core;

namespace WeeXnes.Views.ProfileView
{
    public partial class LoginView : Page
    {
        public static bool alreadyLoggedIn = false;
        public static UpdateVar<string> errorStringCache = new UpdateVar<string>();
        public LoginView()
        {
            InitializeComponent();
        }

        private void SwitchToProfileView()
        {
            NavigationService.Navigate(new Uri("/Views/ProfileView/ProfileView.xaml",UriKind.Relative));
        }

        private void LoginView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (alreadyLoggedIn)
            {
                SwitchToProfileView();
            }
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            attemptLogin();
        }

        private void InputKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                attemptLogin();
        }

        private void attemptLogin()
        {
            ProfileView.auth._email = tb_email.Text;
            ProfileView.auth._password = tb_password.Text;
            SwitchToProfileView();
        }
    }
}