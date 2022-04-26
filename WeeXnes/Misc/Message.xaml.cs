using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeeXnes.Misc
{
    /// <summary>
    /// Interaktionslogik für Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        public Message(string _message)
        {
            InitializeComponent();
            MessageLabel.Content = _message;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
