using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.Home
{
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void VersionInfo_OnLoaded(object sender, RoutedEventArgs e)
        {
            VersionInfo.Text = "WeeXnes Hub v" + Information.Version;
        }
    }
}