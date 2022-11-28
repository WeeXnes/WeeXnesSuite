using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using WeeXnes.Core;
using WeeXnes.Views.DiscordRPC;
using Wpf.Ui.Mvvm.Services;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using MessageBox = System.Windows.MessageBox;

namespace WeeXnes
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void RPCBtn_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (HandleLaunchArguments.Data.Autostart)
            {
                ButtonRpc.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                MainFrame.Source = new Uri("/Views/DiscordRPC/RunRPCView.xaml",UriKind.Relative);
                WindowState = WindowState.Minimized;
            }
        }

        private void ContextStartRpc_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonRpc.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            MainFrame.Source = new Uri("/Views/DiscordRPC/RunRPCView.xaml",UriKind.Relative);
        }

        private void ContextStopRpc_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonHome.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}


