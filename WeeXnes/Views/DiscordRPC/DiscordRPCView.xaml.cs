using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.DiscordRPC
{
    public partial class DiscordRPCView : Page
    {
        public static class Data
        {
            public static BindingList<Game> Games = new BindingList<Game>();
            public static Game SelectedItem = null;
        }
        public DiscordRPCView()
        {
            InitializeComponent();
            ItemboxRpc.ItemsSource = Data.Games;
            Data.Games.ListChanged += GamesOnListChanged;
        }

        private void GamesOnListChanged(object sender, ListChangedEventArgs e)
        {
            foreach (Game game in Data.Games)
            {
                //Save Item to disk
                game.Save();
            }
        }
        
        private void ButtonAddProcess_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/AddRPCView.xaml",UriKind.Relative));
        }

        private void ItemboxRpc_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Data.SelectedItem = (Game)listBox.SelectedItem;
        }

        private void ContextEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if(Data.SelectedItem == null)
                return;
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/EditRPCView.xaml",UriKind.Relative));
        }

        private void ContextRemove_OnClick(object sender, RoutedEventArgs e)
        {
            Game selectedCache = Data.SelectedItem;
            if(selectedCache == null)
                return;
            Data.Games.Remove(selectedCache);
            File.Delete(Global.AppDataPathRPC + "\\" + selectedCache.UUID + ".rpc");
            
        }

        private void ButtonStartRPC_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/RunRPCView.xaml",UriKind.Relative));
        }
    }
}