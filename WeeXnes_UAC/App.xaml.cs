using System;
using System.Windows;
using Microsoft.Win32;

namespace WeeXnes_UAC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static string subkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        public static string autoStartKeyName = "WeeXnes";
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if(e.Args.Length > 0)
            {
                for(int i = 0; i < e.Args.Length; i++)
                {
                    if(e.Args[i] == "-EnableAutostart")
                    {
                        enableAutostart();
                    }
                    if(e.Args[i] == "-DisableAutostart")
                    {
                        disableAutostart();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Arguments");
            }
        }
        
        private void disableAutostart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(subkey);
            try
            {
                key.DeleteValue(autoStartKeyName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            key.Close();
        }

        private void enableAutostart()
        {
            string app = AppDomain.CurrentDomain.BaseDirectory + "Autostart.exe";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(subkey);
            key.SetValue(autoStartKeyName, app);
            key.Close();
        }
    }
}