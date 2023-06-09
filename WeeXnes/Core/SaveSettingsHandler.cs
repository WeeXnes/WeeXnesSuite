using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows;
using WeeXnes.Views.KeyManager;
using WeeXnes.Views.Settings;

namespace WeeXnes.Core
{
    public static class SaveSettingsHandler
    {
        public static class Data
        {
            //Layout-Names for INIFiles
            public static class General
            {
                public const string Section = "GENERAL";
                public const string RpcFilesPath = "RpcFilesPath";
                public const string KeyFilesPath = "KeyFilesPath";
            }
            public static class KeyManager
            {
                public const string Section = "KEY_MANAGER";
                public const string CensorKeys = "CensorKeys";
                public const string CopyOnSelect = "CopyOnSelect";
                public const string SortList = "SortList";
            }
            public static class DiscordRpcFiles
            {
                public const string Section = "CONFIG";
                public const string ProcessName = "ProcessName";
                public const string ClientId = "ClientID";
                public const string State = "State";
                public const string Details = "Details";
                public const string BigImageKey = "BigImageKey";
                public const string BigImageText = "BigImageText";
                public const string SmallImageKey = "SmallImageKey";
                public const string SmallImageText = "SmallImageText";
                public const string UUID = "UUID";
            }
        }
        public static void SetupSaveEvents()
        {
            KeyManagerView.Data.censorKeys.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.KeyManager.Section,
                    Data.KeyManager.CensorKeys, 
                    KeyManagerView.Data.censorKeys.Value.ToString()
                    );
            };
            
            KeyManagerView.Data.copyOnSelect.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.KeyManager.Section,
                    Data.KeyManager.CopyOnSelect, 
                    KeyManagerView.Data.copyOnSelect.Value.ToString()
                );
            };
            
            
            KeyManagerView.Data.sortList.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.KeyManager.Section,
                    Data.KeyManager.SortList, 
                    KeyManagerView.Data.sortList.Value.ToString()
                );
            };
            
            
            Global.AppDataPathRPC.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.General.Section,
                    Data.General.RpcFilesPath,
                    Global.AppDataPathRPC.Value
                );
            };
            Global.AppDataPathKEY.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.General.Section,
                    Data.General.KeyFilesPath,
                    Global.AppDataPathKEY.Value
                );
            };
        }
    }
}