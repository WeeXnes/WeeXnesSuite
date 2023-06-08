using System.Windows;
using Wpf.Ui.Controls;

namespace WeeXnes.Views.Settings
{
    public partial class FluentMessageBox : UiWindow
    {
        public FluentMessageBox(string messageContent)
        {
            InitializeComponent();
            ErrorDump.Content = "Exception: " + messageContent;
        }
    }
}