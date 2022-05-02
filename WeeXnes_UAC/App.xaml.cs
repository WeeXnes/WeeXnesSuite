using System;
using System.IO;
using System.Windows;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;

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

                    if (e.Args[i] == "-CreateStartMenuShortcut")
                    {
                        AddShortcut();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Arguments");
            }
        }
        private static void AddShortcut()
        {
            
            string path = Application.StartupPath;
            string fileName = "WeeXnes.exe";
            string pathToExe = path + "\\" + fileName;
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            Console.WriteLine(commonStartMenuPath);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "WeeXnes");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = Path.Combine(appStartMenuPath, "WeeXnes Suite" + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "WeeXnes Tool Suite";
            //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
            shortcut.TargetPath = pathToExe;
            shortcut.Save(); 
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