using System.Windows;
using System.Windows.Input;

namespace WeeXnes.Misc
{
    public partial class CriticalMessage : Window
    {
        public CriticalMessage(string _message, string _title = "Message")
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