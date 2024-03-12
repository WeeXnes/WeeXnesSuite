using System;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Net;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;
using WeeXnes.Views.DiscordRPC;
using WeeXnes.Views.KeyManager;
using WeeXnes.Views.Settings;
using Application = System.Windows.Forms.Application;

namespace WeeXnes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static bool DebugMode = false;
        private void SetExceptionHandler()
        {
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
        }
        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            using (StreamWriter writer = new StreamWriter("error_log.txt"))
            {
                writer.WriteLine(formattedDateTime);
                writer.WriteLine(ex.ToString());
            }
            new FluentMessageBox(ex.Message).ShowDialog();
        }
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Environment.CurrentDirectory = Application.StartupPath;
            Console.Data.Colors.colored_output = false;
            Console.Data.Formatting.timestamp_prefix = true;
            SetExceptionHandler();
            CheckForDebugMode();
            CheckStartupArgs(e.Args);
            CheckUpdatedFiles();
            CheckForFolder();
            LoadSettings();
            SaveSettingsHandler.SetupSaveEvents();
            LoadFiles();
        }

        private void CheckUpdatedFiles()
        {
            string[] files = System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.new");
            foreach (string file in files)
            {
                try
                {
                    string originalFile = file.Substring(0, file
                        .Length - 4);
                    if (File.Exists(originalFile))
                        File.Delete(originalFile);
                    System.IO.File.Move(file, originalFile);
                }
                catch (Exception ex)
                {
                    Console.Error(ex.ToString());
                }
            }
        }

        private void LoadSettings()
        {
            if(!File.Exists(Path.Combine(Global.AppDataPath, Global.SettingsFile)))
                return;
            KeyManagerView.Data.censorKeys.Value =
                Convert.ToBoolean(SettingsView.Data.settingsFile.GetValue(
                    SaveSettingsHandler.Data.KeyManager.Section,
                    SaveSettingsHandler.Data.KeyManager.CensorKeys));

            KeyManagerView.Data.copyOnSelect.Value =
                Convert.ToBoolean(SettingsView.Data.settingsFile.GetValue(
                    SaveSettingsHandler.Data.KeyManager.Section,
                    SaveSettingsHandler.Data.KeyManager.CopyOnSelect));
            
            KeyManagerView.Data.sortList.Value =
                Convert.ToBoolean(SettingsView.Data.settingsFile.GetValue(
                    SaveSettingsHandler.Data.KeyManager.Section,
                    SaveSettingsHandler.Data.KeyManager.SortList));
            
            //Load paths

            string customRpcPath = SettingsView.Data.settingsFile.GetValue(
                SaveSettingsHandler.Data.General.Section,
                SaveSettingsHandler.Data.General.RpcFilesPath
            );
            if (!String.IsNullOrEmpty(customRpcPath))
            {
                Global.AppDataPathRPC.Value = customRpcPath;
            }
            else
            {
                Global.AppDataPathRPC.Value = Global.Defaults.DefaultPathRPC;
            }
            string customKeyPath = SettingsView.Data.settingsFile.GetValue(
                SaveSettingsHandler.Data.General.Section,
                SaveSettingsHandler.Data.General.KeyFilesPath
            );
            if (!String.IsNullOrEmpty(customKeyPath))
            {
                Global.AppDataPathKEY.Value = customKeyPath;
            }
            else
            {
                Global.AppDataPathKEY.Value = Global.Defaults.DefaultPathKEY;
            }
        }

        private void LoadFiles()
        {
            Functions.CheckFolderAndCreate(Global.AppDataPathRPC.Value);
            DirectoryInfo rpcDirectoryInfo = new DirectoryInfo(Global.AppDataPathRPC.Value);
            foreach (var file in rpcDirectoryInfo.GetFiles("*.rpc"))
            {
                try
                {
                    Game newGame = Game.Methods.GameFromIni(new INIFile(file.FullName));
                    DiscordRPCView.Data.Games.Add(newGame);
                    Console.WriteLine(file.Name + " loaded -> " + newGame.ProcessName);
                }
                catch (Exception ex)
                {
                    Console.Error(file.Name + ": " + ex.Message);
                    new FluentMessageBox(file.Name + ": " + ex.Message).ShowDialog();
                }
            }
            Functions.CheckFolderAndCreate(Global.AppDataPathKEY.Value);
            DirectoryInfo keyDirectoryInfo = new DirectoryInfo(Global.AppDataPathKEY.Value);
            foreach (var file in keyDirectoryInfo.GetFiles("*.wx"))
            {
                try
                {
                    //Load KeyFiles
                    
                    WXFile wxFile = new WXFile(file.FullName);
                    KeyItem newItem = new KeyItem(
                        wxFile.GetName(), 
                        EncryptionLib.EncryptorLibary.decrypt(
                            Information.EncryptionHash,
                            wxFile.GetValue()
                            )
                    );
                    newItem.Filename = file.Name;
                    KeyManagerView.Data.KeyItemsList.Add(newItem);
                    Console.WriteLine(file.Name + " loaded -> " + newItem.Name);
                    
                }
                catch (Exception ex)
                {
                    Console.Error(file.Name + ": " + ex.Message);
                    new FluentMessageBox(file.Name + ": " + ex.Message).ShowDialog();
                }
            }
        }
        private void CheckForFolder()
        {
            Functions.CheckFolderAndCreate(Global.AppDataPath);
        }
        private void CheckStartupArgs(string[] arguments)
        {
            foreach (string argument in arguments)
            {
                switch (argument)
                {
                    case HandleLaunchArguments.ArgumentStrings.autostart:
                        HandleLaunchArguments.arg_autostart();
                        break;
                    case HandleLaunchArguments.ArgumentStrings.debugMode:
                        HandleLaunchArguments.arg_debugMode();
                        break;
                }
            }
        }
        
        private void CheckForDebugMode()
        {
#if DEBUG
                HandleLaunchArguments.arg_debugMode();
#endif
        }
    }
}