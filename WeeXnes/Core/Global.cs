using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WeeXnes.Core
{
    public class Information
    {
        public const string Version = "4.4.1";
        public const string EncryptionHash = "8zf5#RdyQ]$4x4_";
        public const string ApiUrl = "https://api.github.com/repos/weexnes/weexnessuite/releases/latest";
    }
    
    public class Global
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeeXnes");
        public static UpdateVar<string> AppDataPathRPC = new UpdateVar<string>();
        public static UpdateVar<string> AppDataPathKEY = new UpdateVar<string>();
        public static string SettingsFile = "settings.ini";
        public class Defaults
        {
            public static string DefaultPathRPC = Path.Combine(AppDataPath, "RPC");
            public static string DefaultPathKEY = Path.Combine(AppDataPath, "Keys");
        }
        public static void ForceClose()
        {
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }
    }
}