using System;
using System.Windows;
using System.Windows.Controls;
using WeeXnes.Core;

namespace WeeXnes.Views.PasswordGenerator
{
    public partial class PasswordGenView : Page
    {
        public static Random random = new Random();
        private class PasswordBuilderStrings
        {
            public static string alphabetical_lc = "abcdefghijklmnopqrstuvwxyz";
            public static string alphabetical_caps = "ABCDEFGHIJKLMNOPQRSTUVQXYZ";
            public static string numerical = "1234567890";
            public static string special = "!#$%&()*+,-<>=?";
        }
        public PasswordGenView()
        {
            InitializeComponent();
        }

        private void GeneratePasword()
        {
            if(toggle_alpha.IsChecked == false)
                if(toggle_numeric.IsChecked == false)
                    if(toggle_special.IsChecked == false)
                        return;
            
            
            string CustomPasswordBuilderString = "";
            
            if (toggle_alpha.IsChecked == true)
                CustomPasswordBuilderString += PasswordBuilderStrings.alphabetical_lc;
            
            if (toggle_caps.IsChecked == true)
                CustomPasswordBuilderString += PasswordBuilderStrings.alphabetical_caps;
            
            if (toggle_numeric.IsChecked == true)
                CustomPasswordBuilderString += PasswordBuilderStrings.numerical;
            
            if (toggle_special.IsChecked == true)
                CustomPasswordBuilderString += PasswordBuilderStrings.special;
            

            CustomConsole.WriteLine("Generating Password from: " + CustomPasswordBuilderString);
            //MessageBox.Show(CustomPasswordBuilderString);

            string generatedPassword = "";


            for (int i = 0; i < numbox_pwCount.Value; i++)
            {
                int randomNr = random.Next(0, CustomPasswordBuilderString.Length);
                generatedPassword = generatedPassword + CustomPasswordBuilderString[randomNr];
            }

            SavePasswordView.GeneratedPassword = generatedPassword;
            NavigationService.Navigate(new Uri("/Views/PasswordGenerator/SavePasswordView.xaml",UriKind.Relative));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            GeneratePasword();
        }
    }
}