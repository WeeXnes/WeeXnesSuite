using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WeeXnes.Views.EncryptedTextEditor;

public partial class TextEditorView : Page
{
    public TextEditorView()
    {
        InitializeComponent();
    }

    private void Btn_openFile_OnClick(object sender, RoutedEventArgs e)
    {
        var fileContent = string.Empty;
        var filePath = string.Empty;

        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "WXN Text Files (*.wtf)|*.wtf";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
    }

    private void Btn_saveFile_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void flkpw(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}