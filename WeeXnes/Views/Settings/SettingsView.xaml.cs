using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using Application = System.Windows.Forms.Application;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;
using WeeXnes.Views.KeyManager;

namespace WeeXnes.Views.Settings
{
    public partial class SettingsView : Page
    {
        public static class Data
        {
            public static INIFile settingsFile = new INIFile(Global.AppDataPath + "\\" + Global.SettingsFile, true);
            
        }
        public SettingsView()
        {
            InitializeComponent();
            LoadSettingsToGui();
            EnableDebugOptions();
        }

        private void EnableDebugOptions()
        {
            if(!App.DebugMode)
                return;
            
            DebugOptions.Visibility = Visibility.Visible;
        }
        private void LoadSettingsToGui()
        {
            CheckboxCensorKeys.IsChecked = KeyManagerView.Data.censorKeys.Value;
            CheckboxCopyOnSelect.IsChecked = KeyManagerView.Data.copyOnSelect.Value;
            CheckboxSortKeys.IsChecked = KeyManagerView.Data.sortList.Value;
        }
        private void CheckboxCensorKeys_OnChecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.censorKeys.Value = true;
        }

        private void CheckboxCensorKeys_OnUnchecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.censorKeys.Value = false;
        }



        private void ButtonCheckForUpdates_OnClick(object sender, RoutedEventArgs e)
        {
            
            try
            {
                using(WebClient webClient = new WebClient())
                {;
                    webClient.Headers.Add("Authorization", "Basic :x-oauth-basic");
                    webClient.Headers.Add("User-Agent","lk-github-clien");
                    var downloadString = webClient.DownloadString(Information.ApiUrl);
                    GithubApiResponse apiResponseData = JsonConvert.DeserializeObject<GithubApiResponse>(downloadString);
                    if (apiResponseData.tag_name !=  Information.Version)
                    {
                        UpdateFoundView.Data.updateResponse = apiResponseData;
                        NavigationService.Navigate(new Uri("/Views/Settings/UpdateFoundView.xaml",UriKind.Relative));
                    }
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void ButtonEnableRPC_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process uac_prompt = Process.Start("WeeXnes_UAC.exe", "-EnableAutostart");
                uac_prompt.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonDisableRPC_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process uac_prompt = Process.Start("WeeXnes_UAC.exe", "-DisableAutostart");
                uac_prompt.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonCreateStartMenuShortcut_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process p = Process.Start("WeeXnes_UAC.exe", "-CreateStartMenuShortcut");
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangePathsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings/ChangePathsView.xaml",UriKind.Relative));
        }

        private void CheckboxCopyOnSelect_OnChecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.copyOnSelect.Value = true;
        }

        private void CheckboxCopyOnSelect_OnUnchecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.copyOnSelect.Value = false;
        }

        private void CheckboxSortKeys_OnChecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.sortList.Value = true;
        }

        private void CheckboxSortKeys_OnUnchecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.sortList.Value = false;
        }

        private void dbg_throwException(object sender, RoutedEventArgs e)
        {
            Functions.ThrowTestException(new ArithmeticException());
        }

        private void TimedShutdown_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -s -t " + (Convert.ToInt32(ShutdownTimer.Value)*60).ToString();
            process.StartInfo = startInfo;
            process.Start();
        }

        private void ResetShutdown_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown -a";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}