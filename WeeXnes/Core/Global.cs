using System;
using System.Collections.Generic;
using System.IO;

namespace WeeXnes.Core
{
    public class Information
    {
        public const string Version = "4.0.1";
        public const string EncryptionHash = "8zf5#RdyQ]$4x4_";
        public const string ApiUrl = "https://api.github.com/repos/weexnes/weexnessuite/releases/latest";
    }
    
    public class Global
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeeXnes");
        public static string AppDataPathRPC = Path.Combine(AppDataPath, "RPC");
        public static string AppDataPathKEY = Path.Combine(AppDataPath, "Keys");
        public static string SettingsFile = "settings.ini";
    }
}