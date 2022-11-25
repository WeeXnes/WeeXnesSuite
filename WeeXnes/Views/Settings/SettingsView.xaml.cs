using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using Application = System.Windows.Forms.Application;
using System.Windows.Controls;
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
            public static GithubApiResponse updateResponse = null;
        }
        public SettingsView()
        {
            InitializeComponent();
            LoadSettingsToGui();
        }

        private void LoadSettingsToGui()
        {
            CheckboxCensorKeys.IsChecked = KeyManagerView.Data.censorKeys.Value;
        }
        private void CheckboxCensorKeys_OnChecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.censorKeys.Value = true;
        }

        private void CheckboxCensorKeys_OnUnchecked(object sender, RoutedEventArgs e)
        {
            KeyManagerView.Data.censorKeys.Value = false;
        }

        private void DialogUpdate_OnButtonLeftClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Do Update");
            if(Data.updateResponse == null)
                return;
            
            if (File.Exists(Data.updateResponse.assets[0].name))
            {
                File.Delete(Data.updateResponse.assets[0].name);
            }
            using(WebClient webClient = new WebClient())
            {;
                
                webClient.DownloadFile(
                    Data.updateResponse.assets[0].browser_download_url, 
                    Data.updateResponse.assets[0].name
                    );
            }  
            try
            {
                string path = Application.StartupPath;
                string fileName = Path.GetFileName(Application.ExecutablePath);
                string pid = Process.GetCurrentProcess().Id.ToString();
                Process updateProc = Process.Start("Update.exe", "\"" + path + "\"" + " " + "\"" + fileName + "\"" + " " + "\"" + pid + "\"" + " " + "\"" + Data.updateResponse.assets[0].name + "\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }

        private void DialogUpdate_OnButtonRightClick(object sender, RoutedEventArgs e)
        {
            DialogUpdate.Hide();
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
                        Console.WriteLine("Update blabla " + apiResponseData.tag_name);
                        Data.updateResponse = apiResponseData;
                        DialogUpdate.Content = apiResponseData.tag_name + " is avaiable";
                        DialogUpdate.Show();
                    }
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}