using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.DiscordRPC
{
    public partial class RunRPCView : Page
    {
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public RunRPCView()
        {
            InitializeComponent();
            SetupBackgroundWorker();
        }

        private void SetupBackgroundWorker()
        {
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            RunBackgroundWorker();
        }
        public void RunBackgroundWorker()
        {
            try
            {
                if(!backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void StopBackgroundWorker()
        {
            
            try
            {
                if(backgroundWorker.IsBusy)
                    backgroundWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //Stop RPC
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            bool runWorker = true;
            while (runWorker)
            {
                Process[] processes = Process.GetProcesses();
                if (backgroundWorker.CancellationPending)
                    runWorker = false;
                
                foreach (Game game in DiscordRPCView.Data.Games)
                {
                    game.CheckState(processes);
                }
                Thread.Sleep(2000);
            }
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Game game in DiscordRPCView.Data.Games)
                game.Stop();
            Console.WriteLine("Thread Stopped");
        }

        private void RunRPCView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopBackgroundWorker();
        }

        private void ButtonRPCStop_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/DiscordRPCView.xaml",UriKind.Relative));
        }
    }
}