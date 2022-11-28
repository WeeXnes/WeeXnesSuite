using System.Collections.Specialized;
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
                public const string Autostart = "Autostart";
            }
            public static class KeyManager
            {
                public const string Section = "KEY_MANAGER";
                public const string CensorKeys = "CensorKeys";
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
            SettingsView.Data.Autostart.ValueChanged += () =>
            {
                SettingsView.Data.settingsFile.SetValue(
                    Data.General.Section,
                    Data.General.Autostart, 
                    SettingsView.Data.Autostart.Value.ToString()
                );
            };
        }
    }
}