using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using WeeXnes.Core;
using Application = System.Windows.Forms.Application;

namespace WeeXnes.Misc
{
    public partial class UpdateMessage : Window
    {
        public static ApiResponse GitHub;
        public UpdateMessage(ApiResponse _GitHub, string _title = "Message")
        {
            InitializeComponent();
            string content = "Your Version: " + Globals.version + "\n" +
                             "Current Version: " + _GitHub.tag_name;
            MessageLabel.Content = content;
            this.Title = _title;
            GitHub = _GitHub;
        }

        public static void downloadAssets()
        {
            checkForFile();
            WebClient client = new WebClient();
            client.DownloadFile(GitHub.download_url, GitHub.file_name);
        }
        private static void checkForFile()
        {
            if (File.Exists(GitHub.file_name))
            {
                File.Delete(GitHub.file_name);
            }
        }
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            downloadAssets();
            try
            {
                string path = Application.StartupPath;
                string fileName = Path.GetFileName(Application.ExecutablePath);
                string pid = Process.GetCurrentProcess().Id.ToString();
                Process updateProc = Process.Start("Update.exe", "\"" + path + "\"" + " " + "\"" + fileName + "\"" + " " + "\"" + pid + "\"" + " " + "\"" + GitHub.file_name + "\"");
            }
            catch (Exception ex)
            {
                Misc.Message message = new Misc.Message(ex.ToString());
                message.Show();

            }
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}