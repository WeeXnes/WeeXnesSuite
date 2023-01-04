﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.DiscordRPC
{
    public partial class RunRPCView : Page
    {
        public static class Data
        {
            public static UpdateVar<string> LogCache = new UpdateVar<string>();
        }
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public RunRPCView()
        {
            InitializeComponent();
            SetupLogListener();
            SetupBackgroundWorker();
        }
        public void SetupLogListener()
        {
            Data.LogCache.ValueChanged += LogChanged;
        }

        public void RemoveListener()
        {
            Data.LogCache.ValueChanged -= LogChanged;
        }

        private void LogChanged()
        {
            Console.WriteLine("Log Write Data: " + Data.LogCache.Value);
            this.Dispatcher.Invoke(() =>
            {
                RichTextBoxRPCLog.AppendText(Data.LogCache.Value + "\n");
                RichTextBoxRPCLog.ScrollToEnd();
            });
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
            Data.LogCache.Value = "RPC Thread is running";
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
            Data.LogCache.Value = "RPC Thread has stopped";
        }

        private void RunRPCView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopBackgroundWorker();
            RemoveListener();
        }

        private void ButtonRPCStop_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/DiscordRPCView.xaml",UriKind.Relative));
        }
    }
}