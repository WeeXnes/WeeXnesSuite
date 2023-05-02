using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.KeyManager
{
    public partial class KeyManagerView : Page
    {
        public static class Data
        {
            public static BindingList<KeyItem> KeyItemsList = new BindingList<KeyItem>();
            public static UpdateVar<bool> censorKeys = new UpdateVar<bool>();
            public static UpdateVar<bool> copyOnSelect = new UpdateVar<bool>();
        }
        public KeyManagerView()
        {
            InitializeComponent();
            ListviewKeys.ItemsSource = Data.KeyItemsList;
            //((INotifyCollectionChanged)listview_keys.Items).CollectionChanged += OnCollectionChanged;
        }
        private void KeyManagerView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ListviewKeys.Items.Refresh();
        }

        private void Btn_add_OnClick(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(tb_keyname.Text))
                return;
            if(String.IsNullOrEmpty(tb_keyvalue.Text))
                return;

            try
            {
                KeyItem newKey = new KeyItem(
                    tb_keyname.Text,
                    tb_keyvalue.Text
                );
                WXFile wxFile = new WXFile(
                    Global.AppDataPathKEY.Value + "\\" + newKey.Filename);
                WXFile.Methods.WriteFile(newKey, wxFile);
                Data.KeyItemsList.Add(newKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ClearInputs();
        }

        private void ClearInputs()
        {
            tb_keyname.Text = "";
            tb_keyvalue.Text = "";
        }

        private void DeleteItem(KeyItem removeItem)
        {
            if(removeItem ==  null)
                return;

            Data.KeyItemsList.Remove(removeItem);
            try
            {
                File.Delete(Global.AppDataPathKEY.Value + "\\" + removeItem.Filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteItem((KeyItem)ListviewKeys.SelectedItem);
        }

        private void KeyValue_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!Data.censorKeys.Value)
                return;
            
            TextBlock tb = (TextBlock)sender;
            string censoredString = "";
            for (int i = 0; i <= tb.Text.Length; i++)
                censoredString = censoredString + "•";
            tb.Text = censoredString;
            
            
        }

        private void ListviewKeys_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyItem selectedItem = (KeyItem)ListviewKeys.SelectedItem;
            if(selectedItem == null)
                return;
            
            if(!Data.copyOnSelect.Value)
                return;
            Clipboard.SetText(selectedItem.Value);
        }
    }
}