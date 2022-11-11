using WeeXnes.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nocksoft.IO.ConfigFiles;
using MessageBox = System.Windows.MessageBox;

namespace WeeXnes.Core
{
    internal class Globals
    {
        public static string encryptionKey = "8zf5#RdyQ]$4x4_";
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeeXnes");
        public static string SettingsFileName = "settings.ini";
        public static string version = "3.6.9";
        public static bool   info_isRpcRunning = false;
        public static bool   info_RpcAutoStart;
        public static string apiUrl = "https://api.github.com/repos/weexnes/weexnessuite/releases/latest";

        public static UpdateVar<bool>   settings_alwaysOnTop                 = new UpdateVar<bool>();
        public static UpdateVar<bool>   settings_osxStyleControlls           = new UpdateVar<bool>();

        public static UpdateVar<bool>   settings_copySelectedToClipboard     = new UpdateVar<bool>(); 
        
        public static string            settings_KeyManagerItemsPath_Default = AppDataPath + "\\" + "Keys";
        public static UpdateVar<string> settings_KeyManagerItemsPath         = new UpdateVar<string>();
        public static UpdateVar<bool>   settings_KeyManagerItemsPath_Bool    = new UpdateVar<bool>();
        public static UpdateVar<bool>   settings_KeyManagerCensorKeys        = new UpdateVar<bool>();

        public static string            settings_RpcItemsPath_Default        = AppDataPath + "\\" + "RPC";
        public static UpdateVar<string> settings_RpcItemsPath                = new UpdateVar<string>();
        public static UpdateVar<bool>   settings_RpcItemsPath_Bool           = new UpdateVar<bool>();
        
        public static UpdateVar<bool>   settings_RpcShowElapsedTime          = new UpdateVar<bool>();
        public static UpdateVar<string> settings_RpcDefaultClientID          = new UpdateVar<string>();
        public static UpdateVar<bool>   settings_RpcAutoStart                = new UpdateVar<bool>();



        public static UpdateVar<string> searchbox_content = new UpdateVar<string>();
    }
    public static class SettingsManager
    {
        public static INIFile SettingsFile = new INIFile(
            Globals.AppDataPath + "\\" + Globals.SettingsFileName, 
            true
            );
        public static void start()
        {
            loadSettings();
            setEventListeners();
        }

        private static void loadSettings()
        {
            Globals.settings_alwaysOnTop.Value = Convert.ToBoolean(SettingsFile.GetValue("general", "alwaysOnTop"));
            
            Globals.settings_copySelectedToClipboard.Value = Convert.ToBoolean(SettingsFile.GetValue("KeyManager", "copySelectedToClipboard"));
            Globals.settings_KeyManagerItemsPath_Bool.Value = Convert.ToBoolean(SettingsFile.GetValue("KeyManager", "KeyManagerItemsPath_Bool"));
            Globals.settings_KeyManagerCensorKeys.Value = Convert.ToBoolean(SettingsFile.GetValue("KeyManager", "CensorKeys"));
            if (Globals.settings_KeyManagerItemsPath_Bool.Value)
            {
                Globals.settings_KeyManagerItemsPath.Value = SettingsFile.GetValue("KeyManager", "KeyManagerItemsPath");
            }
            else
            {
                Globals.settings_KeyManagerItemsPath.Value = Globals.settings_KeyManagerItemsPath_Default;
            }
            
            Globals.settings_osxStyleControlls.Value = Convert.ToBoolean(SettingsFile.GetValue("general", "OSXStyle"));
            
            Globals.settings_RpcShowElapsedTime.Value = Convert.ToBoolean(SettingsFile.GetValue("rpc", "RpcShowElapsedTime"));
            Globals.settings_RpcItemsPath_Bool.Value = Convert.ToBoolean(SettingsFile.GetValue("rpc", "RpcItemsPath_Bool"));
            Globals.settings_RpcAutoStart.Value = Convert.ToBoolean(SettingsFile.GetValue("rpc", "RpcAutoStart"));
            if (Globals.settings_RpcItemsPath_Bool.Value)
            {
                Globals.settings_RpcItemsPath.Value = SettingsFile.GetValue("rpc", "RpcItemsPath");
            }
            else
            {
                Globals.settings_RpcItemsPath.Value = Globals.settings_RpcItemsPath_Default;
            }

            Globals.settings_RpcDefaultClientID.Value = SettingsFile.GetValue("rpc", "RpcDefaultClientID");
            if (String.IsNullOrEmpty(Globals.settings_RpcDefaultClientID.Value))
            {
                Globals.settings_RpcDefaultClientID.Value = "605116707035676701";
            }


        }

        private static void setEventListeners()
        {
            Globals.settings_KeyManagerItemsPath_Bool.ValueChanged += () =>
            {
                if (Globals.settings_KeyManagerItemsPath_Bool.Value)
                {
                    SettingsFile.SetValue("KeyManager", "KeyManagerItemsPath_Bool", "true");
                }
                else
                {
                    SettingsFile.SetValue("KeyManager", "KeyManagerItemsPath_Bool", "false");
                }
            };
            Globals.settings_KeyManagerItemsPath.ValueChanged += () =>
            {
                if (Globals.settings_KeyManagerItemsPath_Bool.Value)
                {
                    SettingsFile.SetValue("KeyManager", "KeyManagerItemsPath", Globals.settings_KeyManagerItemsPath.Value);
                }
                else
                {
                    SettingsFile.SetValue("KeyManager", "KeyManagerItemsPath", "");
                }
            };
            
            
            Globals.settings_KeyManagerCensorKeys.ValueChanged += () =>
            {
                if (Globals.settings_KeyManagerCensorKeys.Value)
                {
                    SettingsFile.SetValue("KeyManager", "CensorKeys", "true");
                }
                else
                {
                    SettingsFile.SetValue("KeyManager", "CensorKeys", "false");
                }
            };
            Globals.settings_osxStyleControlls.ValueChanged += () =>
            {
                SettingsFile.SetValue("general", "OSXStyle", Globals.settings_osxStyleControlls.Value.ToString());
            };
            
            Globals.settings_RpcItemsPath_Bool.ValueChanged += () =>
            {
                if (Globals.settings_RpcItemsPath_Bool.Value)
                {
                    SettingsFile.SetValue("rpc", "RpcItemsPath_Bool", "true");
                }
                else
                {
                    SettingsFile.SetValue("rpc", "RpcItemsPath_Bool", "false");
                }
            };
            Globals.settings_RpcItemsPath.ValueChanged += () =>
            {
                if (Globals.settings_RpcItemsPath_Bool.Value)
                {
                    SettingsFile.SetValue("rpc", "RpcItemsPath", Globals.settings_RpcItemsPath.Value);
                }
                else
                {
                    SettingsFile.SetValue("rpc", "RpcItemsPath", "");
                }
            };
            Globals.settings_alwaysOnTop.ValueChanged += () =>
            {
                SettingsFile.SetValue("general","alwaysOnTop",Convert.ToString(Globals.settings_alwaysOnTop.Value));
            };
            Globals.settings_copySelectedToClipboard.ValueChanged += () =>
            {
                SettingsFile.SetValue("KeyManager","copySelectedToClipboard",Convert.ToString(Globals.settings_copySelectedToClipboard.Value));
            };
            Globals.settings_RpcDefaultClientID.ValueChanged += () =>
            {
                SettingsFile.SetValue("rpc", "RpcDefaultClientID", Globals.settings_RpcDefaultClientID.Value);
            };
            Globals.settings_RpcShowElapsedTime.ValueChanged += () =>
            {
                SettingsFile.SetValue("rpc", "RpcShowElapsedTime", Convert.ToString(Globals.settings_RpcShowElapsedTime.Value));
            };
            Globals.settings_RpcAutoStart.ValueChanged += () =>
            {
                SettingsFile.SetValue("rpc","RpcAutoStart", Convert.ToString(Globals.settings_RpcAutoStart.Value));
            };

        }
    }
    public static class funcs
    {
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
    public class UpdateVar<T>
    {
        private T _value;

        public Action ValueChanged;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged() => ValueChanged?.Invoke();
    }
}


