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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeeXnes.Keys;
using WeeXnes.Core;
using wx;
using System.IO;
using System.ComponentModel;

namespace WeeXnes.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für KeyManagerView.xaml
    /// </summary>
    public partial class KeyManagerView : UserControl
    {
        

        
        public KeyManagerView()
        {
            CheckForFolders();
            InitializeComponent();
            Globals.searchbox_content.ValueChanged += () => { SearchboxChanged(); };
        }

        public void SearchboxChanged()
        {
            Console.WriteLine("Searchbox: " + Globals.searchbox_content.Value);
            KeyListView.Items.Clear();
            List<KeyItem> items = new List<KeyItem>();
            foreach(KeyItem item in KeyManagerLib.KeyList)
            {
                if(Contains(item.name, Globals.searchbox_content.Value, StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(item);
                }
                
            }
            if(items.Count > 0)
            {
                foreach(KeyItem item in items)
                {
                    KeyListView.Items.Add(item);
                }
            }
            else
            {
                KeyListView.Items.Clear();
            }
        }
        public bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        #region ListControls


        public void FillList()
        {
            Console.WriteLine("->Filling Listview");
            KeyListView.Items.Clear();
            foreach (KeyItem key in KeyManagerLib.KeyList)
            {
                KeyListView.Items.Add(key);
            }
        }

        public void AddItemFromUI()
        {
            if (!String.IsNullOrEmpty(Textbox_Name.Text))
            { 
                if (!String.IsNullOrEmpty(Textbox_Value.Text))
                {
                    KeyItem newkey = new KeyItem(Textbox_Name.Text, Textbox_Value.Text);
                    KeyManagerLib.KeyList.Add(newkey);
                    string filename = Globals.settings_KeyManagerItemsPath.Value + "\\" + Guid.NewGuid().ToString() + ".wx";
                    string[] filecontent = new string[] { "##WXfile##", newkey.name, EncryptionLib.EncryptorLibary.encrypt(Globals.encryptionKey, newkey.value) };
                    /*
                    INIFile newini = new INIFile(filename, true);
                    newini.SetValue("key", "name", newkey.name);
                    newini.SetValue("key", "value", newkey.value);
                    */
                    EncryptionLib.EncryptorLibary.writeFile(filecontent, filename);
                    Console.WriteLine("Added: <" + newkey.name + "> Value: " + newkey.value);
                }
            }
        }
        public void AddItem(string _name, string _value)
        {
            KeyItem newkey = new KeyItem(_name, _value);
            KeyManagerLib.KeyList.Add(newkey);
        }

        public void PrintList()
        {
            Console.WriteLine("-------------------------------");
            foreach (KeyItem item in KeyManagerLib.KeyList)
            {
                Console.WriteLine(item.name + ": " + item.value);
            }
        }


        #endregion

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            KeyManagerLib.KeyList.Clear();
            if (!SaveInterface.IsDirectoryEmpty(Globals.settings_KeyManagerItemsPath.Value))
            {
                string[] files = SaveInterface.GetFilesInDir(Globals.settings_KeyManagerItemsPath.Value);
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    try
                    {
                        wxfile inifile = new wxfile(file);
                        string name = inifile.GetName();
                        string value = inifile.GetValue();
                        value = EncryptionLib.EncryptorLibary.decrypt(Globals.encryptionKey, value);
                        if (name != null && value != null)
                        {
                            KeyItem newitem = new KeyItem(name, value);
                            KeyManagerLib.KeyList.Add(newitem);
                            Console.WriteLine("Added Item: <" + newitem.name + ">");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    FillList();
                }
            }
        }

        private void CheckForFolders()
        {
            if (!Globals.settings_KeyManagerItemsPath_Bool.Value)
            {
                Globals.settings_KeyManagerItemsPath.Value = Globals.settings_KeyManagerItemsPath_Default;
            }
            if (!Directory.Exists(Globals.AppDataPath))
            {
                Directory.CreateDirectory(Globals.AppDataPath);
                Console.WriteLine("Created AppDataPath");
            }
            if (!Directory.Exists(Globals.settings_KeyManagerItemsPath.Value))
            {
                Directory.CreateDirectory(Globals.settings_KeyManagerItemsPath.Value);
                Console.WriteLine("Created settings_KeyManagerItemsPath");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemFromUI();
            FillList();
            ClearInputs();
        }

        

        private void DebugBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintList();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            FillList();
        }

        private void KeyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyItem selectedItem = (KeyItem)KeyListView.SelectedItem;
            if(selectedItem != null)
            {
                Console.WriteLine(selectedItem.name + ": " + selectedItem.value);
                if (Globals.settings_copySelectedToClipboard.Value)
                {
                    Clipboard.SetText(selectedItem.value);
                }
                Console.WriteLine("Copied: " + selectedItem.value + " to Clipboard");
            }
        }

        private void KeyListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            KeyItem selectedItem = (KeyItem)KeyListView.SelectedItem;
            Console.WriteLine("Doubleclicked " + selectedItem.name);
            KeyManagerLib.KeyList.Remove(selectedItem);
            string[] files = SaveInterface.GetFilesInDir(Globals.settings_KeyManagerItemsPath.Value);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                try
                {
                    wxfile inifile = new wxfile(file);
                    string name = inifile.GetName();
                    string value = inifile.GetValue();
                    if(name == selectedItem.name)
                    {
                        File.Delete(file);
                        Console.WriteLine("Removed File: " + file);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            FillList();
        }
        private void ClearInputs()
        {
            Textbox_Name.Clear();
            Textbox_Value.Clear();
        }

        private void addNameClip_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("fnmgikegnmek");
        }

    }
    public static class SaveInterface
    {
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public static string[] GetFilesInDir(string path)
        {
            return Directory.GetFiles(path, "*.wx");
        }


    }
    
}
