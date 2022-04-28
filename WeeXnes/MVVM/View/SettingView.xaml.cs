using System;
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
            LoadUiFromSettingsFile();
            SetFunction();
            UpdatePathsOnUi();
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

        private void LoadUiFromSettingsFile()
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            if (Globals.settings_alwaysOnTop)
            {
                AlwaysOnTopSwitch.IsChecked = true;
            }
            if (Globals.settings_RpcShowElapsedTime)
            {
                ShowElapsedTimeOnRpc.IsChecked = true;
            }

            if (Globals.settings_copySelectedToClipboard)
            {
                ItemToClipboardSwitch.IsChecked = true;
            }
            bool autoStartRpc = Convert.ToBoolean(SettingsFile.GetValue("RPC", "autoStartRpc"));
            if (autoStartRpc)
            {
                EnableAutoStart.IsChecked = true;
            }


            tb_DefaultClientID.Text = Globals.settings_RpcDefaultClientID;
        }
        public static void CheckSetting()
        {
            

            if(!File.Exists(Globals.AppDataPath + "\\" + Globals.SettingsFileName))
            {
                INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
                SettingsFile.SetValue("General", "AlwaysOnTop", "false");
                SettingsFile.SetValue("RPC", "showElapsedTime", "true");
                SettingsFile.SetValue("KeyManager", "copyToClipboard", "false");
                SettingsFile.SetValue("RPC", "defaultID", "605116707035676701");
                CheckSetting();
            }
            else
            {
                INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName);

                Globals.settings_alwaysOnTop = Convert.ToBoolean(SettingsFile.GetValue("General", "AlwaysOnTop"));
                Console.WriteLine(Globals.settings_alwaysOnTop);
                Globals.settings_RpcShowElapsedTime = Convert.ToBoolean(SettingsFile.GetValue("RPC", "showElapsedTime"));
                Console.WriteLine(Globals.settings_RpcShowElapsedTime);
                Globals.settings_copySelectedToClipboard = Convert.ToBoolean(SettingsFile.GetValue("KeyManager", "copyToClipboard"));
                Console.WriteLine(Globals.settings_copySelectedToClipboard);


                Globals.settings_RpcDefaultClientID = SettingsFile.GetValue("RPC", "defaultID");
                Console.WriteLine(Globals.settings_RpcDefaultClientID);
                
                
                Globals.settings_alwaysOnTop = Convert.ToBoolean(SettingsFile.GetValue("General", "AlwaysOnTop"));
                
                
                Globals.settings_KeyManagerItemsPath_Bool = Convert.ToBoolean(SettingsFile.GetValue("KeyFiles", "CustomKeyLocation"));
                if (Globals.settings_KeyManagerItemsPath_Bool)
                {
                    Globals.settings_KeyManagerItemsPath = SettingsFile.GetValue("KeyFiles", "KeyPath");
                }
                else
                {
                    Globals.settings_KeyManagerItemsPath = Globals.settings_KeyManagerItemsPath_Default;
                }
                
                
                Globals.settings_RpcItemsPath_Bool = Convert.ToBoolean(SettingsFile.GetValue("rpc", "CustomRpcLocation"));
                if (Globals.settings_RpcItemsPath_Bool)
                {
                    Globals.settings_RpcItemsPath = SettingsFile.GetValue("rpc", "RpcPath");
                }
                else
                {
                    Globals.settings_RpcItemsPath = Globals.settings_RpcItemsPath_Default;
                }

            }


            /*
            SettingsFile.SetValue("settings", "alwaysOnTop", "false");
            Globals.alwaysOnTop = false;
            SettingsFile.SetValue("RPC", "elapsedTime", "true");
            Globals.showElapsedTime = true;
            */
        }
        private void AlwaysOnTopSwitch_Checked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("General", "AlwaysOnTop", "true");
            CheckSetting();
        }


        private void AlwaysOnTopSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("General", "AlwaysOnTop", "false");
            CheckSetting();
        }

        private void ShowElapsedTimeOnRpc_Checked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("RPC", "showElapsedTime", "true");
            CheckSetting();
        }

        private void ShowElapsedTimeOnRpc_Unchecked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("RPC", "showElapsedTime", "false");
            CheckSetting();
        }

        private void ItemToClipboardSwitch_Checked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("KeyManager", "copyToClipboard", "true");
            CheckSetting();
        }

        private void ItemToClipboardSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("KeyManager", "copyToClipboard", "false");
            CheckSetting();
        }

        private void OpenAppdataFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Globals.AppDataPath);
        }

        private void SaveDefaultID_Click(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            if (!String.IsNullOrEmpty(tb_DefaultClientID.Text))
            {
                SettingsFile.SetValue("RPC", "defaultID", tb_DefaultClientID.Text);
                CheckSetting();
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

            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            if (on)
            {
                SettingsFile.SetValue("RPC", "autoStartRpc", "true");
                EnableAutoStart.IsChecked = true;
            }
            else
            {
                SettingsFile.SetValue("RPC", "autoStartRpc", "false");
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

        private void UpdatePathsOnUi()
        {
            RpcPathLabel.Content = Globals.settings_RpcItemsPath;
            KeyPathLabel.Content = Globals.settings_KeyManagerItemsPath;
        }

        private void SetKeyLocationDefault_OnClick(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("KeyFiles", "CustomKeyLocation", "false");
            SettingsFile.SetValue("KeyFiles", "KeyPath", "");
            CheckSetting();
            UpdatePathsOnUi();
        }

        private void SetKeyLocation_OnClick(object sender, RoutedEventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
                    SettingsFile.SetValue("KeyFiles", "CustomKeyLocation", "true");
                    SettingsFile.SetValue("KeyFiles", "KeyPath", fbd.SelectedPath);
                    CheckSetting();
                    UpdatePathsOnUi();
                    //MessageBox.Show("valid path: " + fbd.SelectedPath);
                }
            }
        }

        private void SetRpcLocationDefault_OnClick(object sender, RoutedEventArgs e)
        {
            INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
            SettingsFile.SetValue("rpc", "CustomRpcLocation", "false");
            SettingsFile.SetValue("rpc", "RpcPath", "");
            CheckSetting();
            UpdatePathsOnUi();
        }

        private void SetRpcLocation_OnClick(object sender, RoutedEventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    INIFile SettingsFile = new INIFile(Globals.AppDataPath + "\\" + Globals.SettingsFileName, true);
                    SettingsFile.SetValue("rpc", "CustomRpcLocation", "true");
                    SettingsFile.SetValue("rpc", "RpcPath", fbd.SelectedPath);
                    CheckSetting();
                    UpdatePathsOnUi();
                    //MessageBox.Show("valid path: " + fbd.SelectedPath);
                }
            }
        }
    }
}
