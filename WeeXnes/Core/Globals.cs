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
        public static string DefaultKeyListPath = AppDataPath + "\\" + "Keys";
        public static string DefaultRpcListPath = AppDataPath + "\\" + "RPC";
        public static string KeyListPath = DefaultKeyListPath;
        public static string RpcListPath = DefaultRpcListPath;
        public static bool KeyCustomPath = false;
        public static bool RpcCustomPath = false;
        public static string SettingsFileName = "settings.ini";
        public static string version = "2.4";
        public static bool isRpcRunning = false;
        public static string defaultRpcClient;
        public static bool alwaysOnTop;
        public static bool showElapsedTime;
        public static bool copySelectedToClipboard; 
        public static bool autoStartRpc;
        

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
