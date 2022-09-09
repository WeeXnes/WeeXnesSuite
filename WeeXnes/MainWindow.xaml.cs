using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using WeeXnes.Core;
using WeeXnes.MVVM.View;

namespace WeeXnes
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
        private ContextMenuStrip trayIconMenu = new ContextMenuStrip();
        public MainWindow()
        {
            buildTrayMenu();
            trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            trayIcon.Visible = false;
            trayIcon.DoubleClick += TrayIcon_DoubleClick;
            trayIcon.ContextMenuStrip = trayIconMenu;
            InitializeComponent();
        }

        private void buildTrayMenu()
        {
            trayIconMenu.Items.Add("Show Window",null,  (sender, args) =>
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                trayIcon.Visible = false;
            });
            //RPC MENU//////////////////////////////////////////////////////////////////////////////////////
            //monke
            
            ToolStripMenuItem DiscordMenu = new ToolStripMenuItem("DiscordRPC");
            
            
            DiscordMenu.DropDownItems.Add("Stop DiscordRPC",null,  (sender, args) =>
            {
                controllRpcFromTray(false);
            });
            DiscordMenu.DropDownItems.Add("Start DiscordRPC",null,  (sender, args) =>
            {
                controllRpcFromTray(true);
            });

            trayIconMenu.Items.Add(DiscordMenu);
            ////////////////////////////////////////////////////////////////////////////////////////////
            trayIconMenu.Items.Add("Exit",null, (sender, args) =>
            {
                this.Close();
            });
            trayIconMenu.Opening += (sender, args) =>
            {
                
            };

        }


        public void controllRpcFromTray(bool start)
        {
            
            //set tray controlls.
            if (start)
            {
                HomeMenuButton.Command.Execute(null);
                HomeMenuButton.IsChecked = true;
                Globals.info_RpcAutoStart = true;
                RpcMenuButton.Command.Execute(null);
                RpcMenuButton.IsChecked = true;
            }
            else
            {
                HomeMenuButton.Command.Execute(null);
                HomeMenuButton.IsChecked = true;
            }
        }
        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            trayIcon.Visible = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckForFolders();
            CheckForSettingsFile();
            CheckForAutoStartup();
        }

        private void CheckForSettingsFile()
        {
            //SettingView.CheckSetting();
            SettingsManager.start();
            if (Globals.settings_osxStyleControlls.Value)
            {
                OSXControlls.Visibility = Visibility.Visible;
            }
            else
            {
                MinimizeBtn.Visibility = Visibility.Visible;
                CloseBtn.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.searchbox_content.Value = Searchbox.Text;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.info_isRpcRunning)
            {
                WindowState = WindowState.Minimized;
            }
            else
            {
                this.Close();
            }
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CheckForAutoStartup()
        {
            if (Globals.info_RpcAutoStart)
            {
                WindowState = WindowState.Minimized;
                RpcMenuButton.Command.Execute(null);
                RpcMenuButton.IsChecked = true;
            }
        }

        private void CheckForFolders()
        {
            if (!Directory.Exists(Globals.AppDataPath))
            {
                Directory.CreateDirectory(Globals.AppDataPath);
                Console.WriteLine("Created AppDataPath");
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            if (Globals.settings_alwaysOnTop.Value)
            {
                window.Topmost = true;
            }
            else
            {
                window.Topmost = false;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
                trayIcon.Visible = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            trayIcon.Dispose();
        }
    }
}


