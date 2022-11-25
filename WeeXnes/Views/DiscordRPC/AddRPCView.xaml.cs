using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.DiscordRPC
{
    public partial class AddRPCView : Page
    {
        public AddRPCView()
        {
            InitializeComponent();
        }

        private void CloseDialog()
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/DiscordRPCView.xaml",UriKind.Relative));
        }

        private void ButtonSaveDialog_OnClick(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(TextboxProcessname.Text))
                return;
            if(String.IsNullOrEmpty(TextboxClientid.Text))
                return;

            try
            {
                //Add new item
                Game newGame = new Game(
                    TextboxProcessname.Text,
                    TextboxClientid.Text,
                    TextboxDetails.Text,
                    TextboxState.Text,
                    TextboxBigimgkey.Text,
                    TextboxBigimgtxt.Text,
                    TextboxSmallimgkey.Text,
                    TextboxSmallimgtxt.Text
                );
                DiscordRPCView.Data.Games.Add(newGame);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            CloseDialog();
            
        }

        private void ButtonCancelDialog_OnClick(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }
    }
}