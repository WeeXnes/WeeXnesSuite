using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.ProfileView
{
    public partial class LicenseView : Page
    {
        public LicenseView()
        {
            InitializeComponent();
        }

        private void LicenseView_OnLoaded(object sender, RoutedEventArgs e)
        {
            header_label.Content = "Licenses of " + ProfileView.auth._currentUserCache.Value.name;
        }
    }
}