using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Release_Tool
{
    internal class Program
    {
        static List<file> files = new List<file>();
        public static string globalTimestamp = null;
        public static string releaseFolder = "debug_release";
        public static string releaseFileName = "currentRelease.zip";
        public static string destFolder = null;
        public static bool success = true;
        public static void Main(string[] args)
        {
            SetTimestamp();
            SetPaths();
            Console.WriteLine("Folder -> " + globalTimestamp);
            CheckDirectories();
            GetFiles();
            PackFiles();
            if (!success)
            {
                Console.WriteLine("Something went wrong");
                Console.ReadLine();
            }
            else
            {
                PackIntoZip();
                Console.WriteLine("Build succeeded | " + globalTimestamp);
                Console.ReadLine();
            }
        }
        
        private static void PackIntoZip()
        {
            try
            {
                if(File.Exists(Path.Combine(releaseFolder, releaseFileName)))
                {
                    File.Delete(Path.Combine(releaseFolder, "currentRelease.zip"));
                    ZipFile.CreateFromDirectory(destFolder, Path.Combine(releaseFolder, releaseFileName));
                }
                else
                {
                    ZipFile.CreateFromDirectory(destFolder, Path.Combine(releaseFolder, releaseFileName));
                }
                
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static void SetPaths()
        {
            destFolder = Path.Combine(releaseFolder, globalTimestamp);
        }

        private static void CheckDirectories()
        {
            if (!Directory.Exists(releaseFolder))
            {
                Directory.CreateDirectory(releaseFolder);
            }
            if(!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
        }

        private static void SetTimestamp()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy");
            string time = DateTime.Now.ToString("HH.mm.ss");
            string timestamp = date + " - " + time;
            globalTimestamp = timestamp;
        }

        private static void PackFiles()
        {
            foreach(file fileobj in files)
            {
                try
                {
                    File.Copy(fileobj.path, Path.Combine(destFolder, fileobj.newfilename));
                    Console.WriteLine("Copied " + fileobj.path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Coudnt find " + fileobj.path + "| ->" + ex.GetType());
                    success = false;
                }
            }
        }

        private static void GetFiles()
        {
            files.Add(new file(@"WeeXnes\bin\Debug\WeeXnes.exe", "WeeXnes.exe"));
            files.Add(new file(@"WeeXnes_UAC\bin\Debug\WeeXnes_UAC.exe", "WeeXnes_UAC.exe"));
            files.Add(new file(@"WeeXnes\bin\Debug\DiscordRPC.dll", "DiscordRPC.dll"));
            files.Add(new file(@"WeeXnes\bin\Debug\Newtonsoft.Json.dll", "Newtonsoft.Json.dll"));
            files.Add(new file(@"Autostart\bin\Debug\Autostart.exe", "Autostart.exe"));
        }
    }
}