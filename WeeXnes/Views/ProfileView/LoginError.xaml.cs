using System;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.ProfileView
{
    public partial class LoginError : Page
    {
        public LoginError()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ProfileView/LoginView.xaml",UriKind.Relative));
        }
    }
}