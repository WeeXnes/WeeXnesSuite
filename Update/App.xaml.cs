using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace Update
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            RunUpdater(e.Args[0],e.Args[1],e.Args[2],e.Args[3]);
        }
        private void RunUpdater(string Path, string FileName, string ProcessId, string NewFile)
        {
            if(String.IsNullOrEmpty(Path))
                return;
            if(String.IsNullOrEmpty(FileName))
                return;
            if(String.IsNullOrEmpty(ProcessId))
                return;
            if(String.IsNullOrEmpty(NewFile))
                return;
            
            Process p = Process.GetProcessById(Convert.ToInt32(ProcessId));
            p.Kill();
            p.WaitForExit();
            try
            {
                ZipArchiveHelper.ExtractToDirectory(NewFile, Path, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ZipArchiveHelper.ExtractToDirectory(NewFile, Path, true);
            }
            Process.Start(Path + "\\" + FileName);
            if (File.Exists(NewFile))
            {
                File.Delete(NewFile);
            }

            Environment.Exit(0);
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
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex);
                            var completeFileName = Path.Combine(destinationDirectoryName, file.FullName + ".new");
                            file.ExtractToFile(completeFileName);
                        }
                    }

                }
            }            
        }
    }
}