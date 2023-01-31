using System;
using System.IO;
using System.Threading;
using System.Security.AccessControl;

namespace TempDeleter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TempDeleter | Starting...";

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"Attempting to delete files in: \"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\".\r\n");

            foreach (FileInfo file in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp").GetFiles())
            {
                try
                {
                    FileSecurity fileSecurity = file.GetAccessControl();
                    fileSecurity.SetOwner(System.Security.Principal.WindowsIdentity.GetCurrent().User);
                    file.SetAccessControl(fileSecurity);

                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unable to delete: \"{file.Name}\", file may be in use. (Exception: \"{ex.Message}\")");
                    Console.Title = $"TempDeleter | Unable to delete: \"{file.Name}\", file may be in use.";
                }
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\r\nAttempting to delete folders/directories in: \"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\".\r\n");

            foreach (DirectoryInfo info in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp").GetDirectories())
            {
                try
                {
                    DirectorySecurity dirSecurity = info.GetAccessControl();
                    dirSecurity.SetOwner(System.Security.Principal.WindowsIdentity.GetCurrent().User);
                    info.SetAccessControl(dirSecurity);

                    Directory.Delete(info.FullName, true);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unable to delete: \"{info.Name}\", folder/directory may be in use. (Exception: \"{ex.Message}\")");
                    Console.Title = $"TempDeleter | Unable to delete: \"{info.Name}\", folder may be in use.";
                }
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------------\r\n\r\nFinished, exiting in 1 second.");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
