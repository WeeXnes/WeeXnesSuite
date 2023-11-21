using System;
using System.Runtime.InteropServices;
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
            MessageBox.Show("user debug mode enabled");
        }
        public static void arg_enableConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }
    }
}