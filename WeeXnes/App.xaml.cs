using System;
using System.ComponentModel;
using System.Windows;
using System.IO;
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
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Environment.CurrentDirectory = Application.StartupPath;
            CheckForDebugMode();
            CheckUpdatedFiles();
            CheckForFolder();
            LoadSettings();
            SaveSettingsHandler.SetupSaveEvents();
            LoadFiles();
            CheckStartupArgs(e.Args);
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
                    Console.WriteLine(ex);
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
            
        }

        private void LoadFiles()
        {
            Functions.CheckFolderAndCreate(Global.AppDataPathRPC);
            DirectoryInfo rpcDirectoryInfo = new DirectoryInfo(Global.AppDataPathRPC);
            foreach (var file in rpcDirectoryInfo.GetFiles("*.rpc"))
            {
                try
                {
                    Game newGame = Game.Methods.GameFromIni(new INIFile(file.FullName));
                    DiscordRPCView.Data.Games.Add(newGame);
                    Console.WriteLine(file.Name + " loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(file.Name + ": " + ex.Message);
                    MessageBox.Show(file.Name + ": " + ex.Message);
                }
            }
            Functions.CheckFolderAndCreate(Global.AppDataPathKEY);
            DirectoryInfo keyDirectoryInfo = new DirectoryInfo(Global.AppDataPathKEY);
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
                    Console.WriteLine(file.Name + " loaded");
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(file.Name + ": " + ex.Message);
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
                    case "-autostart":
                        HandleLaunchArguments.arg_autostart();
                        break;
                    case "-debugMode":
                        HandleLaunchArguments.arg_debugMode();
                        break;
                }
            }
        }
        
        private void CheckForDebugMode()
        {
            #if DEBUG
                HandleLaunchArguments.arg_enableConsole();
            #endif
        }
    }
}