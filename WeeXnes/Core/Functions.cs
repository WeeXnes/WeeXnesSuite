using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace WeeXnes.Core
{
    public static class Functions
    {
        public static void CheckFolderAndCreate(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static string[] readFile(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            var listOfStrings = new List<string>();
            foreach (string line in lines)
            {
                listOfStrings.Add(line);
            }
            string[] arrayOfStrings = listOfStrings.ToArray();
            return arrayOfStrings;
        }
        public static void writeFile(string[] stringArray, string filepath)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                Console.WriteLine(stringArray[i]);
            }
            File.WriteAllLines(filepath, stringArray, Encoding.UTF8);
        }
    }
}