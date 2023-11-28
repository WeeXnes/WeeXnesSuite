using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WeeXnes.Core
{
    public class HandleLaunchArguments
    {
        public static class Data
        {
            public static bool Autostart = false;
        }
        private const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        public static void arg_autostart()
        {
            Data.Autostart = true;
        }
        public static void arg_debugMode()
        {
            App.DebugMode = true;
            HandleLaunchArguments.arg_enableConsole();
            //Allow untrusted certs in Debug mode
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }
        public static void arg_enableConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }
    }
}