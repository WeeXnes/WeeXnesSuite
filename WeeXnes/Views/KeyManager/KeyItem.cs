using System;
using System.Windows.Shapes;
using WeeXnes.Core;
using System.IO;
using System.Net;
using Wpf.Ui.Controls;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;

namespace WeeXnes.Views.KeyManager
{
    public class KeyItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Filename { get; set; }
        public KeyItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.Filename = Guid.NewGuid().ToString() + ".wx";
        }

        public override string ToString()
        {
            return this.Name;
        }


        public void Export()
        {
            string filePath = Global.AppDataPathKEY.Value + "\\" + this.Filename;
            Console.WriteLine(filePath);

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "WXFiles (*.wx)|*.wx",
                Title = "Export KeyFile"
            };
            if (dialog.ShowDialog() == true)
            {
                File.Copy(filePath, dialog.FileName, true);
                Console.WriteLine("Exported to: " + dialog.FileName);
            }
        }

        public static void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "WXFiles (*.wx)|*.wx"
            };
            if (dialog.ShowDialog() == true)
            {
                WXFile wxFile = new WXFile(dialog.FileName);
                KeyItem newItem = new KeyItem(
                    wxFile.GetName(), 
                    EncryptionLib.EncryptorLibary.decrypt(
                        Information.EncryptionHash,
                        wxFile.GetValue()
                    )
                );
                newItem.Filename = System.IO.Path.GetFileName(dialog.FileName);
                WXFile newWxFile = new WXFile(Global.AppDataPathKEY.Value + "\\" + newItem.Filename);
                WXFile.Methods.WriteFile(newItem, newWxFile);
                KeyManagerView.Data.KeyItemsList.Add(newItem);
                Console.WriteLine("Imported: " + dialog.FileName);
            }
        }
    }
}