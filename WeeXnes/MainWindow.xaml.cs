using System;
using System.Windows;
using WeeXnes.Core;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using NotifyIcon = Wpf.Ui.Controls.NotifyIcon;

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

        private void NotifyIcon_OnLeftClick(NotifyIcon sender, RoutedEventArgs e)
        {
            this.Show();
        }
    }
}


