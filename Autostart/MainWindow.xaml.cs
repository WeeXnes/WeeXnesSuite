using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Autostart
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        UpdateVar<int> startupProgressInt = new UpdateVar<int>();
        public MainWindow()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            runOnStartup();
        }
        public void runOnStartup()
        {
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
            startupProgressInt.ValueChanged += () =>
            {
                changeProgressBar();
            };
            worker.RunWorkerAsync();
        }

        private void changeProgressBar()
        {
            this.Dispatcher.Invoke(() =>
            {
                StartupProgress.Value = startupProgressInt.Value;
            });
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            for (int i = 0; i < 200; i++)
            {
                startupProgressInt.Value = i;
                Thread.Sleep(25);
            }

            try
            {
                Process.Start("WeeXnes.exe", "-autostart");
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show(Directory.GetCurrentDirectory());
                //MessageBox.Show(Environment.CurrentDirectory);
            }
            Thread.Sleep(500);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
    public class UpdateVar<T>
    {
        private T _value;

        public Action ValueChanged;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged() => ValueChanged?.Invoke();
    }
}
