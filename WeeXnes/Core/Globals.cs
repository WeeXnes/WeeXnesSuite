using WeeXnes.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nocksoft.IO.ConfigFiles;

namespace WeeXnes.Core
{
    internal class Globals
    {
        public static string encryptionKey = "8zf5#RdyQ]$4x4_";
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeeXnes");
        public static string SettingsFileName = "settings.ini";
        public static string version = "2.4";
        public static bool isRpcRunning = false;
        
        public static bool   settings_alwaysOnTop;
        
        public static bool   settings_copySelectedToClipboard; 
        
        public static string settings_KeyManagerItemsPath_Default = AppDataPath + "\\" + "Keys";
        public static bool   settings_KeyManagerItemsPath_Bool = false;
        public static string settings_KeyManagerItemsPath = settings_KeyManagerItemsPath_Default;
        
        public static string settings_RpcItemsPath_Default = AppDataPath + "\\" + "RPC";
        public static string settings_RpcItemsPath = settings_RpcItemsPath_Default;
        public static bool   settings_RpcItemsPath_Bool = false;
        
        public static bool   settings_RpcShowElapsedTime;
        public static string settings_RpcDefaultClientID;
        
        public static bool   info_RpcAutoStart;
        
        

        public static UpdateVar<string> searchbox_content = new UpdateVar<string>();
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
