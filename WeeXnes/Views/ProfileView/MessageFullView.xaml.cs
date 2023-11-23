using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WeeXnes.Views.ProfileView
{
    public partial class MessageFullView : Page
    {
        public static dynamic MessageToShow = null;
        public MessageFullView()
        {
            InitializeComponent();
        }   

        private void MessageFullView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(MessageToShow == null)
                return;
            AuthorLabel.Content = MessageToShow.Author;
            MessageContent.Document.Blocks.Clear();
            MessageContent.Document.Blocks.Add(
                new Paragraph(
                    new Run(
                        MessageToShow.Message.ToString()
                        )
                    )
                );
        }
    }
}