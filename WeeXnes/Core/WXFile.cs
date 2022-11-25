using System;
using System.Collections.Generic;
using WeeXnes.Views.KeyManager;

namespace WeeXnes.Core
{
    public class WXFile
    {
        private const string FileIdentifier = "##WXfile##";
        public string path
        {
            get; 
            set;
        }
        public WXFile(string _path)
        {
            this.path = _path;
        }
        public string GetName()
        {
            string returnval = null;
            string[] rawcontent = Methods.ReadFile(this.path);

            if (Methods.Verify(rawcontent))
            {
                try
                {
                    returnval = rawcontent[1];
                }
                catch (Exception e)
                {
                    returnval = null;
                }
            }

            return returnval;
        }
        public string GetValue()
        {
            string returnval = null;
            string[] rawcontent = Methods.ReadFile(this.path);

            if (Methods.Verify(rawcontent))
            {
                try
                {
                    returnval = rawcontent[2];
                }catch (Exception e)
                {
                    returnval = null;
                }
            }

            return returnval;
        }
        public static class Methods
        {
            public static void WriteFile(KeyItem keyItem, WXFile wxFile)
            {
                
                string[] fileContent = new string[]
                {
                    "##WXfile##", 
                    keyItem.Name, 
                    EncryptionLib.EncryptorLibary.encrypt(
                        Information.EncryptionHash, 
                        keyItem.Value
                        )
                };
                Functions.writeFile(fileContent, wxFile.path);
            }
            public static string[] ReadFile(string filepath)
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
            public static bool Verify(string[] content)
            {
                bool integ = false;
                if(content !=  null)
                {
                    if(content[0] ==  FileIdentifier)
                    {
                        integ = true;
                    }
                }
                return integ;
            }
        }
    }
}