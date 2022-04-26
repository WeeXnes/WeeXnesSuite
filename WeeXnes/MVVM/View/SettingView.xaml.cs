using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;

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
            if (Globals.alwaysOnTop)
            {
                AlwaysOnTopSwitch.IsChecked = true;
            }
            if (Globals.showElapsedTime)
            {
                ShowElapsedTimeOnRpc.IsChecked = true;
            }

            if (Globals.copySelectedToClipboard)
            {
                ItemToClipboardSwitch.IsChecked = true;
            }
            bool autoStartRpc = Convert.ToBoolean(SettingsFile.GetValue("RPC", "autoStartRpc"));
            if (autoStartRpc)
            {
                EnableAutoStart.IsChecked = true;
            }


            tb_DefaultClientID.Text = Globals.defaultRpcClient;
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

                Globals.alwaysOnTop = Convert.ToBoolean(SettingsFile.GetValue("General", "AlwaysOnTop"));
                Console.WriteLine(Globals.alwaysOnTop);
                Globals.showElapsedTime = Convert.ToBoolean(SettingsFile.GetValue("RPC", "showElapsedTime"));
                Console.WriteLine(Globals.showElapsedTime);
                Globals.copySelectedToClipboard = Convert.ToBoolean(SettingsFile.GetValue("KeyManager", "copyToClipboard"));
                Console.WriteLine(Globals.copySelectedToClipboard);


                Globals.defaultRpcClient = SettingsFile.GetValue("RPC", "defaultID");
                Console.WriteLine(Globals.defaultRpcClient);

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
    }
}
