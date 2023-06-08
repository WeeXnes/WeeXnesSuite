using System;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;
using WeeXnes.Views.KeyManager;

namespace WeeXnes.Views.PasswordGenerator
{
    public partial class SaveToKeyManagerView : Page
    {
        
        public SaveToKeyManagerView()
        {
            InitializeComponent();
        }
        private void SavePassword(object sender, RoutedEventArgs e)
        {
            try
            {
                KeyItem newKey = new KeyItem(
                    tb_keyname.Text,
                    SavePasswordView.GeneratedPassword
                );
                WXFile wxFile = new WXFile(
                    Global.AppDataPathKEY.Value + "\\" + newKey.Filename);
                WXFile.Methods.WriteFile(newKey, wxFile);
                KeyManagerView.Data.KeyItemsList.Add(newKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            NavigationService.Navigate(SavePasswordView._prevPage);
        }
    }
}