using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;
using CConsole = WeeXnes.Core.CustomConsole;

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

        private void ContextMenu_Edit(object sender, RoutedEventArgs e)
        {
            if(Data.SelectedItem == null)
                return;
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/EditRPCView.xaml",UriKind.Relative));
        }
        private void ContextMenu_Export(object sender, RoutedEventArgs e)
        {
            Game selectedCache = Data.SelectedItem;
            if(selectedCache == null)
                return;
            string filepath = Global.AppDataPathRPC.Value + "\\" + selectedCache.UUID + ".rpc";
            
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = selectedCache.UUID, 
                Filter = "RPC File (*.rpc)|*.rpc",
                Title = "Export RPC File"
            };
            if (dialog.ShowDialog() == true)
            {
                File.Copy(filepath, dialog.FileName, true);
                CustomConsole.WriteLine("Exported to: " + dialog.FileName);
            }
            
        }
        private void ContextMenu_Import(object sender, RoutedEventArgs e)
        {
            Game selectedCache = Data.SelectedItem;
            if(selectedCache == null)
                return;
            
            
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "RPC File (*.rpc)|*.rpc",
                Title = "Import RPC File"
            };
            if (dialog.ShowDialog() == true)
            {
                
                Game newGame = Game.Methods.GameFromIni(new INIFile(dialog.FileName));
                
                if (!File.Exists(Global.AppDataPathRPC.Value + "\\" + newGame.UUID + ".rpc"))
                {
                    File.Copy(dialog.FileName, Global.AppDataPathRPC.Value + "\\" + newGame.UUID + ".rpc", true);
                    DiscordRPCView.Data.Games.Add(newGame);
                    CustomConsole.WriteLine("Imported: " + dialog.FileName);
                }
                else
                {
                    CustomConsole.Error("not imported: " + dialog.FileName);
                }
                
                
            }
            
        }

        private void ContextMenu_Remove(object sender, RoutedEventArgs e)
        {
            Game selectedCache = Data.SelectedItem;
            if(selectedCache == null)
                return;
            Data.Games.Remove(selectedCache);
            File.Delete(Global.AppDataPathRPC.Value + "\\" + selectedCache.UUID + ".rpc");
            
        }

        private void ButtonStartRPC_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/RunRPCView.xaml",UriKind.Relative));
        }
    }
}