using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeXnes.Core;

namespace WeeXnes.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand KeyManagerViewCommand { get; set; }
        public RelayCommand DiscordRpcViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }



        public HomeViewModel HomeVM { get; set; }
        public KeyManagerViewModel KeyManagerVM { get; set; }
        public DiscordRpcViewModel DiscordRpcVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            KeyManagerVM = new KeyManagerViewModel();
            DiscordRpcVM = new DiscordRpcViewModel();
            SettingsVM = new SettingsViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            KeyManagerViewCommand = new RelayCommand(o =>
            {
                CurrentView = KeyManagerVM;
            });
            DiscordRpcViewCommand = new RelayCommand(o =>
            {
                CurrentView = DiscordRpcVM;
            });
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });
        }
    }
}
