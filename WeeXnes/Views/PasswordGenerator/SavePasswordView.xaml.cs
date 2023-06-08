using System;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.PasswordGenerator
{
    public partial class SavePasswordView : Page
    {
        public static string GeneratedPassword = "";
        public SavePasswordView()
        {
            InitializeComponent();
        }

        private void SavePasswordView_OnLoaded(object sender, RoutedEventArgs e)
        {
            displayPassword.Content = GeneratedPassword;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/PasswordGenerator/PasswordGenView.xaml",UriKind.Relative));
        }
        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GeneratedPassword);
        }
        private void SaveToKeyManager(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/PasswordGenerator/SaveToKeyManagerView.xaml",UriKind.Relative));
        }
    }
}