using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using EncryptionLib;
using WeeXnes.Core;

namespace WeeXnes.Views.EncryptedTextEditor;

public partial class TextEditorView : Page
{
    public string currentFilePath { get; set; } = null;
    public TextEditorView()
    {
        InitializeComponent();
    }
    private void RaiseClickEvent(System.Windows.Controls.Button button)
    {
        var clickEventArgs = new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent);
        button.RaiseEvent(clickEventArgs);
    }

    private void Btn_openFile_OnClick(object sender, RoutedEventArgs e)
    {
        var fileContent = string.Empty;
        var filePath = string.Empty;

        Console.WriteLine("Calling OpenFileDialog");
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "WXN Text Files (*.wtf)|*.wtf";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("Opening file " + openFileDialog.FileName);
                this.currentFilePath = openFileDialog.FileName;
                string[] FileContent = File.ReadAllLines(openFileDialog.FileName);
                string[] decryptedContent = EncryptorLibary.decryptArray(Information.EncryptionHash, FileContent);
                rtb_FileEditor.Document.Blocks.Clear();
                foreach (string line in decryptedContent)
                {
                    rtb_FileEditor.Document.Blocks.Add(new Paragraph(new Run(line)));
                }
            }
        }
    }
    

    private void Btn_saveFile_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.currentFilePath == null)
        {
            RaiseClickEvent(btn_saveFileAs);
        }
        else
        {
            Console.WriteLine("Saving file " + currentFilePath);
            TextRange textRange = new TextRange(rtb_FileEditor.Document.ContentStart, rtb_FileEditor.Document.ContentEnd);
            string plainText = textRange.Text;
            string[] lines = plainText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] encryptedContent =
                EncryptionLib.EncryptorLibary.encryptArray(Information.EncryptionHash, lines);
            File.WriteAllLines(this.currentFilePath, encryptedContent);
        }
            
        
        
    }

    private void Btn_saveFileAs_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Calling SaveFileDialog");
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            //saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.Filter = "WXN Text Files (*.wtf)|*.wtf";
            saveFileDialog.RestoreDirectory = true;

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("Saving file " + saveFileDialog.FileName);
                this.currentFilePath = saveFileDialog.FileName;
                TextRange textRange = new TextRange(rtb_FileEditor.Document.ContentStart, rtb_FileEditor.Document.ContentEnd);
                string plainText = textRange.Text;
                string[] lines = plainText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string[] encryptedContent =
                    EncryptionLib.EncryptorLibary.encryptArray(Information.EncryptionHash, lines);
                File.WriteAllLines(saveFileDialog.FileName, encryptedContent);
            }
        }
    }
}