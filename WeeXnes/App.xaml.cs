using System;
using System.Windows;
using WeeXnes.Core;
using System.IO;
using System.Reflection;
using WeeXnes.Misc;
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
            if (e.Args.Length > 0)
            {
                for (int i = 0; i != e.Args.Length; ++i)
                {
                    //MessageBox.Show(e.Args[i]);
                    if (e.Args[i] == "-autostart")
                    {
                        //MessageBox.Show("Launched via autostart");
                        Globals.info_RpcAutoStart = true;
                    }
                }

            }
            //Globals.autoStartRpc = true;
        }
    }
}