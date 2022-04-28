﻿using System.Windows;
using WeeXnes.Core;

namespace WeeXnes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
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