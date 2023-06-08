using System;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.PasswordGenerator
{
    public partial class SavePasswordView : Page
    {
        public static string GeneratedPassword = "";
        public static PasswordGenView _prevPage = null;
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
            if(_prevPage != null)
                NavigationService.Navigate(_prevPage);
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