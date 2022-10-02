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
        public Message(string _message, string _title = "Message")
        {
            InitializeComponent();
            MessageLabel.Content = _message;
            this.Title = _title;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
