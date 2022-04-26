using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EncryptionLib
{
    public static class EncryptorLibary
    {
        public static string encrypt(string hash, string texttenc)
        {
            string returnval = "";
            byte[] data = UTF8Encoding.UTF8.GetBytes(texttenc);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    returnval = Convert.ToBase64String(result, 0, result.Length);
                }
            }
            return returnval;
        }
        public static string decrypt(string hash, string texttenc)
        {
            string returnval = "";
            byte[] data = Convert.FromBase64String(texttenc);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    try
                    {
                        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                        returnval = UTF8Encoding.UTF8.GetString(result);
                    }
                    catch
                    {
                        returnval = "Wrong Hash";
                    }
                }
            }
            return returnval;
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
        public static string[] encryptArray(string hash, string[] arrayToEncrypt)
        {
            for (int i = 0; i < arrayToEncrypt.Length; i++)
            {
                arrayToEncrypt[i] = EncryptorLibary.encrypt(hash, arrayToEncrypt[i]);
            }
            return arrayToEncrypt;
        }
        public static string[] decryptArray(string hash, string[] arrayToDecrypt)
        {
            for (int i = 0; i < arrayToDecrypt.Length; i++)
            {
                arrayToDecrypt[i] = EncryptorLibary.decrypt(hash, arrayToDecrypt[i]);
            }
            return arrayToDecrypt;
        }
    }
}

