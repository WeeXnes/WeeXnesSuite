using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;
using Application = System.Windows.Forms.Application;

namespace WeeXnes.Views.Settings
{
    
    public partial class UpdateFoundView : Page
    {
        public static class Data
        {
            public static GithubApiResponse updateResponse = null;
        }
        public UpdateFoundView()
        {
            InitializeComponent();
        }

        private void ButtonInstallUpdate_OnClick(object sender, RoutedEventArgs e)
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
                Console.WriteLine(ex.ToString());

            }
        }

        private void VersionNumberLoaded_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            tb.Text = "New Version: " + Data.updateResponse.tag_name;
        }

        private void CurrentVersionNumberLoaded_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            tb.Text = "Current Version: " + Information.Version;
        }
    }
}