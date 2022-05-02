using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Update
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Path: " + args[0]);
            Console.WriteLine("FileName: " + args[1]);
            Console.WriteLine("PID: " + args[2]);
            Console.WriteLine("New File: " + args[3]);
            Process p = Process.GetProcessById(Convert.ToInt32(args[2]));
            p.Kill();
            p.WaitForExit();
            try
            {
                ZipArchiveHelper.ExtractToDirectory(args[3], args[0], true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ZipArchiveHelper.ExtractToDirectory(args[3], args[0], true);
            }
            Process.Start(args[0] + "\\" + args[1]);
            if (File.Exists(args[3]))
            {
                File.Delete(args[3]);
            }

        }
    }
    public static class ZipArchiveHelper
    {
        public static void ExtractToDirectory(string archiveFileName, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                ZipFile.ExtractToDirectory(archiveFileName, destinationDirectoryName);
            }
            else
            {
                using (var archive = ZipFile.OpenRead(archiveFileName))
                {

                    foreach (var file in archive.Entries)
                    {
                        try
                        {
                            var completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                            var directory = Path.GetDirectoryName(completeFileName);

                            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
                                Directory.CreateDirectory(directory);

                            if (file.Name != "")
                                file.ExtractToFile(completeFileName, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }

                }
            }            
        }
    }
}