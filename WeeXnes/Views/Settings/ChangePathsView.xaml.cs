using System;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.Settings
{
    public partial class ChangePathsView : Page
    {
        public ChangePathsView()
        {
            InitializeComponent();
        }

        private void ChangePathsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextboxKeyPath.Text = Global.AppDataPathKEY.Value;
            TextboxRPCPath.Text = Global.AppDataPathRPC.Value;
        }

        private void ButtonCancelDialog_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings/SettingsView.xaml",UriKind.Relative));
        }

        private void ButtonSaveDialog_OnClick(object sender, RoutedEventArgs e)
        {
            Global.AppDataPathRPC.Value = TextboxRPCPath.Text;
            Global.AppDataPathKEY.Value = TextboxKeyPath.Text;
            Global.ForceClose();
        }
    }
}