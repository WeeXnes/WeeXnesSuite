using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WeeXnes.Core;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;

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

        private void ContextMenu_SelectKfFolder(object sender, RoutedEventArgs e)
        {
            CallOpenFolderDialog(TextboxKeyPath);
        }
        private void ContextMenu_SelectRpcFolder(object sender, RoutedEventArgs e)
        {
            CallOpenFolderDialog(TextboxRPCPath);
        }

        private void CallOpenFolderDialog(TextBox textBoxToChange)
        {


            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBoxToChange.Text = fbd.SelectedPath;
                }
            }
        }
    }
}