﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace WeeXnes.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
            //LoadUiFromSettingsFile();
            SetFunction();
            SetUiUpdateListeners();
            InitializeUi();
            //UpdatePathsOnUi();
        }

        public void InitializeUi()
        {
            if (!String.IsNullOrEmpty(Globals.settings_RpcItemsPath.Value))
            {
                RpcPathLabel.Content = Globals.settings_RpcItemsPath.Value;
            }
            else
            {
                RpcPathLabel.Content = "Default";
            }
            if (!String.IsNullOrEmpty(Globals.settings_KeyManagerItemsPath.Value))
            {
                KeyPathLabel.Content = Globals.settings_KeyManagerItemsPath.Value;
            }
            else
            {
                KeyPathLabel.Content = "Default";
            }

            if (Globals.settings_alwaysOnTop.Value)
            {
                AlwaysOnTopSwitch.IsChecked = true;
            }

            if (Globals.settings_RpcAutoStart.Value)
            {
                EnableAutoStart.IsChecked = true;
            }

            if (Globals.settings_RpcShowElapsedTime.Value)
            {
                ShowElapsedTimeOnRpc.IsChecked = true;
            }

            if (Globals.settings_copySelectedToClipboard.Value)
            {
                ItemToClipboardSwitch.IsChecked = true;
            }
        }
        private void SetUiUpdateListeners()
        {
            Globals.settings_RpcItemsPath.ValueChanged += () =>
            {
                if (!String.IsNullOrEmpty(Globals.settings_RpcItemsPath.Value))
                {
                    RpcPathLabel.Content = Globals.settings_RpcItemsPath.Value;
                }
                else
                {
                    RpcPathLabel.Content = "Default";
                }
            };
            Globals.settings_KeyManagerItemsPath.ValueChanged += () =>
            {
                if (!String.IsNullOrEmpty(Globals.settings_KeyManagerItemsPath.Value))
                {
                    KeyPathLabel.Content = Globals.settings_KeyManagerItemsPath.Value;
                }
                else
                {
                    KeyPathLabel.Content = "Default";
                }
            };
        }
        private void SetFunction()
        {
            EnableAutoStart.Checked += EnableAutoStart_Checked;
            EnableAutoStart.Unchecked += EnableAutoStart_Unchecked;
        }
        private void UnsetFunction()
        {
            EnableAutoStart.Checked -= EnableAutoStart_Checked;
            EnableAutoStart.Unchecked -= EnableAutoStart_Unchecked;
        }

        
        private void AlwaysOnTopSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Globals.settings_alwaysOnTop.Value = true;
        }


        private void AlwaysOnTopSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.settings_alwaysOnTop.Value = false;
        }

        private void ShowElapsedTimeOnRpc_Checked(object sender, RoutedEventArgs e)
        {
            Globals.settings_RpcShowElapsedTime.Value = true;
        }

        private void ShowElapsedTimeOnRpc_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.settings_RpcShowElapsedTime.Value = false;
        }

        private void ItemToClipboardSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Globals.settings_copySelectedToClipboard.Value = true;
        }

        private void ItemToClipboardSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.settings_copySelectedToClipboard.Value = false;
        }

        private void OpenAppdataFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Globals.AppDataPath);
        }

        private void SaveDefaultID_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tb_DefaultClientID.Text))
            {
                Globals.settings_RpcDefaultClientID.Value = tb_DefaultClientID.Text;
            }
            else
            {
                Misc.Message message = new Misc.Message("Dont leave ClientID empty");
                message.Show();
            }
        }

        public void switchAutoRpc(bool on)
        {
            UnsetFunction();

            if (on)
            {
                Globals.settings_RpcAutoStart.Value = true;
                EnableAutoStart.IsChecked = true;
            }
            else
            {
                Globals.settings_RpcAutoStart.Value = false;
                EnableAutoStart.IsChecked = false;
            }
            SetFunction();
        }
        private void EnableAutoStart_Checked(object sender, RoutedEventArgs e)
        {
            bool proc_suc;
            try
            {
                Process uacpromp = Process.Start("WeeXnes_UAC.exe", "-EnableAutostart");
                uacpromp.WaitForExit();
                proc_suc = true;
            }
            catch (Exception ex)
            {
                Misc.Message message = new Misc.Message(ex.ToString());
                message.Show();
                proc_suc = false;
            }

            switchAutoRpc(proc_suc);
        }

        private void EnableAutoStart_Unchecked(object sender, RoutedEventArgs e)
        {

            bool proc_suc;
            try
            {
                Process uacpromp = Process.Start("WeeXnes_UAC.exe", "-DisableAutostart");
                uacpromp.WaitForExit();
                proc_suc = false;
            }
            catch (Exception ex)
            {
                Misc.Message message = new Misc.Message(ex.ToString());
                message.Show();
                proc_suc = true;

            }

            switchAutoRpc(proc_suc);
        }


        private void SetKeyLocationDefault_OnClick(object sender, RoutedEventArgs e)
        {
            Globals.settings_KeyManagerItemsPath_Bool.Value = false;
            Globals.settings_KeyManagerItemsPath.Value = "";
        }

        private void SetKeyLocation_OnClick(object sender, RoutedEventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    
                    Globals.settings_KeyManagerItemsPath_Bool.Value = true;
                    Globals.settings_KeyManagerItemsPath.Value = fbd.SelectedPath;
                    //MessageBox.Show("valid path: " + fbd.SelectedPath);
                }
            }
        }

        private void SetRpcLocationDefault_OnClick(object sender, RoutedEventArgs e)
        {
            Globals.settings_RpcItemsPath_Bool.Value = false;
            Globals.settings_RpcItemsPath.Value = "";
        }

        private void SetRpcLocation_OnClick(object sender, RoutedEventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Globals.settings_RpcItemsPath_Bool.Value = true;
                    Globals.settings_RpcItemsPath.Value = fbd.SelectedPath;
                }
            }
        }
    }
}
