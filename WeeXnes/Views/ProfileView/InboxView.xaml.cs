using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WeeXnes.Core;
using Wpf.Ui.Controls;

namespace WeeXnes.Views.ProfileView
{
    public partial class InboxView : Page
    {
        public InboxView()
        {
            InitializeComponent();
        }

        private void InboxView_OnLoaded(object sender, RoutedEventArgs e)
        {
            //header_label.Content = "Inbox of " + ProfileView.auth._currentUserCache.Value.name;
        }

        private void InboxItemsLoaded(object sender, RoutedEventArgs e)
        {
            //Reverse the message list
            List<dynamic> messageList = new List<dynamic>();
            foreach (var msg in ProfileView.auth._currentUserCache.Value.inbox)
            {
                messageList.Add(msg);
            }

            messageList.Reverse();
            InboxItems.ItemsSource = messageList;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CardAction button = (CardAction)sender;
            dynamic messageObj = button.Tag;
            MessageFullView.MessageToShow = messageObj;
            NavigationService.Navigate(new Uri("/Views/ProfileView/MessageFullView.xaml",UriKind.Relative));
        }
    }
}