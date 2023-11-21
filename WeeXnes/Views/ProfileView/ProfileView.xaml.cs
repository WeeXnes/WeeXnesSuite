using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WeeXnes.Core;
using WeeXnes.Views.Settings;

namespace WeeXnes.Views.ProfileView
{
    public partial class ProfileView : Page
    {
        public static Auth auth = new Auth(
            "https://weexnes.dev:4000/login", 
            "https://weexnes.dev:4000/user"
        );
        public ProfileView()
        {
            InitializeComponent();
        }
        private void ProfileView_OnLoaded(object sender, RoutedEventArgs e)
        {
            auth.ExceptionCache.ValueChanged += LoginWorkerException;
            auth._currentUserCache.ValueChanged += userCacheChanged;
            LoginView.errorStringCache.ValueChanged += errorStringChanged;
            WeeXnes.Core.CustomConsole.WriteLine("Error Hooks loaded");
            
            WeeXnes.Core.CustomConsole.WriteLine("Event hooks loaded");
            if (auth._currentUserCache.Value == null)
            {
                LoadingScreen.Visibility = Visibility.Visible;
                auth._loginWorker.RunWorkerAsync();
            }
            else
            {
                LoadProfileToGui();
                ProfileContentPanel.Visibility = Visibility.Visible;
            }
        }

        private void userCacheChanged()
        {
            if (auth._currentUserCache.Value != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    LoadProfileToGui();
                    ProfileContentPanel.Visibility = Visibility.Visible;
                    LoadingScreen.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    ProfileContentPanel.Visibility = Visibility.Collapsed;
                    LoadingScreen.Visibility = Visibility.Visible;
                });
            }
        }

        private void LoginWorkerException()
        {
            this.Dispatcher.Invoke(() =>
            {
                new FluentMessageBox(auth.ExceptionCache.Value.Message).ShowDialog();
            });
        }

        private void LoadProfileToGui()
        {
            //Load Profile
            UsernameLabel.Content = Convert.ToString(auth._currentUserCache.Value.name);
            ProfileDesc.Content = Convert.ToString(auth._currentUserCache.Value.profileInfo.bio);
            BannerBackground.ImageSource =
                new BitmapImage(new Uri(Convert.ToString(auth._currentUserCache.Value.profileInfo.bannerUrl)));
        }

        private void ProfileView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            
            auth.ExceptionCache.ValueChanged -= LoginWorkerException;
            auth._currentUserCache.ValueChanged -= userCacheChanged;
            LoginView.errorStringCache.ValueChanged -= errorStringChanged;
            
            WeeXnes.Core.CustomConsole.WriteLine("Event hooks unloaded");
        }
        private void errorStringChanged()
        {
            this.Dispatcher.Invoke(() =>
            {
                NavigationService.Navigate(new Uri("/Views/ProfileView/LoginError.xaml",UriKind.Relative));
            });
        }
    }
}