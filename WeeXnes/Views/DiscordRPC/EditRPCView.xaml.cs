using System;
using System.Windows;
using System.Windows.Controls;

namespace WeeXnes.Views.DiscordRPC
{
    public partial class EditRPCView : Page
    {
        private Game editItem = null;
        public EditRPCView()
        {
            InitializeComponent();
        }
        private void CloseDialog()
        {
            NavigationService.Navigate(new Uri("/Views/DiscordRPC/DiscordRPCView.xaml",UriKind.Relative));
        }

        private void ButtonSaveDialog_OnClick(object sender, RoutedEventArgs e)
        {
            Game editedItem = new Game(
                TextboxProcessname.Text,
                TextboxClientid.Text,
                TextboxDetails.Text,
                TextboxState.Text,
                TextboxBigimgkey.Text,
                TextboxBigimgtxt.Text,
                TextboxSmallimgkey.Text,
                TextboxSmallimgtxt.Text
                );
            editedItem.UUID = editItem.UUID;
            int listIndex = DiscordRPCView.Data.Games.IndexOf(editItem);
            DiscordRPCView.Data.Games[listIndex] = editedItem;
            CloseDialog();
        }

        private void EditRPCView_OnLoaded(object sender, RoutedEventArgs e)
        {
            editItem = DiscordRPCView.Data.SelectedItem;
            TextboxProcessname.Text = editItem.ProcessName;
            TextboxClientid.Text = editItem.PresenceClient.ApplicationID;
            TextboxState.Text = editItem.State;
            TextboxDetails.Text = editItem.Details;
            TextboxBigimgkey.Text = editItem.BigImageKey;
            TextboxBigimgtxt.Text = editItem.BigImageText;
            TextboxSmallimgkey.Text = editItem.SmallImageKey;
            TextboxSmallimgtxt.Text = editItem.SmallImageText;
        }

        private void ButtonCancelDialog_OnClick(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }
    }
}