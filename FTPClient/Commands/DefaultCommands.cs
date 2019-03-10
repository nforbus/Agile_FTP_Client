using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.IO;
using DiffMatchPatch;
using System.Collections.Generic;
using FluentFTP;

namespace FTPClient.Commands
{
    public static class DefaultCommands
    { 
        public static string Login(string address, string username="")
        {
            string returnMessage = "";
            string password = "";
            if (username == "")
            {
                username = "anonymous";
                password = "anonymous";
            }
            else
            {
                System.Console.Write("Enter the password: ");
                password = FTPClient.Console.Console.ReadPassword();
                System.Console.Write('\n');

            }

            try
            {
                FtpClient client = new FtpClient(address)
                {
                    Credentials = new NetworkCredential(username, password)
                };

                client.Connect();

                if (client.IsConnected)
                {
                    Client.serverName = address;
                    Client.clientObject = client;
                    Client.viewingRemote = true;
                    FTPClient.Console.Console.readPrompt = "FTP ("+ FTPClient.Client.clientObject.GetWorkingDirectory() + ")> ";
                    returnMessage = "Connected to " + address;
                }
            }
            catch (Exception e)
            {
                returnMessage = "Connection failed with Exception";
            }

            return returnMessage;
        }
        
        public static string cd(string filePath)
        {
            FTPClient.Client.clientObject.SetWorkingDirectory(filePath);
            FTPClient.Console.Console.readPrompt = "FTP ("+ FTPClient.Client.clientObject.GetWorkingDirectory() + ")> ";
            return "";
        }

        public static string pwd()
        {
            return FTPClient.Client.clientObject.GetWorkingDirectory();
        }

        public static string lr()
        {
            string returnMessage = "";
            string res = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing(Client.clientObject.GetWorkingDirectory()))
                {
                    res += item + "\n";
                }
                returnMessage = res;
            }
            catch (Exception e)
            {
                returnMessage = "Server not connected Or Listing failed with Exception";
            }
            return returnMessage;
        }

        public static string ls()
        {
            string returnMessage = "";
            try
            {
                returnMessage = "";
                var currentdir = System.IO.Directory.GetCurrentDirectory();
                var directories = System.IO.Directory.GetDirectories(currentdir);
                var files = System.IO.Directory.GetFiles(currentdir);
                foreach (string item in directories)
                {
                    var dir = new DirectoryInfo(item);
                    returnMessage += "DIR\t" + dir.Name + "\t Modified :"+dir.LastWriteTime + '\n';
                }
                foreach (string file in files)
                {
                    var time = File.GetLastWriteTime(file);
                    var size = Path.GetFileName(file).Length;
                    returnMessage += "FILE\t"+ Path.GetFileName(file) + "\t("+size +")bytes" + "\t Modified :" + time +'\n';
                }
            }
            catch (Exception e)
            {
                returnMessage = "Listing of local directories and files failed with Exception";
            }
            return returnMessage;
        }

        //Uses absoulute path for target and name changes
        public static string mv(string target, string name)
        {
            string returnMessage = "";
            string res = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing(Client.clientObject.GetWorkingDirectory()))
                {
                    if(item.FullName == target)
                    {
                        Client.clientObject.Rename(target, name);
                    }
                }
                returnMessage = res;
            }
            catch (Exception e)
            {
                returnMessage = "Rename failed with exception";
            }
            return returnMessage;
        }

        //Uses absolute path for target and name changes
        public static string mvLocal(string target, string name)
        {
            string returnMessage = "";
            string res = "";
            try
            {
                var currentdir = System.IO.Directory.GetCurrentDirectory();
                var directories = System.IO.Directory.GetDirectories(currentdir);
                var files = System.IO.Directory.GetFiles(currentdir);

                string qualifiedTarget = currentdir + "\\" + target;
                string qualifiedName = currentdir + "\\" + name;

                foreach (string item in directories)
                {
                    if (item == qualifiedTarget)
                    {
                        Directory.Move(qualifiedTarget, qualifiedName);
                    }
                }
                foreach (string file in files)
                {
                    if (file == qualifiedTarget)
                    {
                        File.Move(qualifiedTarget, qualifiedName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Console.WriteToConsole(e.ToString());
                returnMessage = "Rename failed with exception";
            }
            return returnMessage;
        }

        public static string upload(string source, string destination)
        {
            string returnMessage = "";
            try
            {
               if(FTPClient.Client.clientObject.UploadFile(source, destination))

                returnMessage = "Upload Successful";
               else
                    returnMessage = "Upload failed";

            }
            catch (Exception e)
            {
                returnMessage = "Upload failed exception" + e;
            }
            return returnMessage;
        }

        public static string download(string source, string destination)
        {
            string returnMessage = "";
            try
            {
                Client.clientObject.DownloadFile(source, destination);
                returnMessage = "Download succesful";

            }
            catch (Exception e)
            {
                returnMessage = "Download failed" + e;
            }
            return returnMessage;
        }

        public static string findl(string filename)
        {
            string returnMessage = "";
            try
            {
                foreach (string file in Directory.EnumerateFiles(System.IO.Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories))
                {
                    if (filename == Path.GetFileName(file))
                    {
                        var time = File.GetLastWriteTime(file);
                        var size = Path.GetFileName(file).Length;
                        returnMessage += "FILE\t" + Path.GetFileName(file) + "\t(" + size + ")bytes" + "\t Modified :" + time + '\n';

                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = "The File does not exist";
            }

            return returnMessage;
        }

        //find files on server 
        public static string findr(string filename)
        {
            string returnMessage = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing(Client.clientObject.GetWorkingDirectory(), FtpListOption.Recursive))
                {
                    if (filename == item.Name)
                    {
                        returnMessage += item + "\n";
                    }
                }
            }
            catch (Exception e)
            {
             
                returnMessage = "Failed with Exception";
            }
            return returnMessage;
        }

        //disconnect from server 
        public static string exit()
        {
            string returnMessage = "";
            try
            {
                Client.serverName = null;
                Client.clientObject = null;
            }
            catch (Exception e)
            {
                returnMessage = "Failed with Exception";
            }
            return returnMessage;
        }

        //Presently working for local files only
        public static string diff(string file1, string file2)
        {
            string returnMessage = "";
            try
            {
                string text1 = System.IO.File.ReadAllText(@file1);
                string text2 = System.IO.File.ReadAllText(@file2);

                var dmp = DiffMatchPatchModule.Default;
                List<DiffMatchPatch.Diff> diff = dmp.DiffMain(text1, text2);

                // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
                dmp.DiffCleanupSemantic(diff);
                // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
                for (int i = 0; i < diff.Count; i++)
                {
                    returnMessage += diff[i] + " ";
                }

            }
            catch (Exception e)
            {
                returnMessage = "Failed";
            }
            return returnMessage;
        }

        public static string uploadMultiple(string files, string destination)
        {
            try
            {
                List<string> args = FTPClient.Console.Console.SeparateArguments(files);

                foreach (var arg in args)
                {
                    System.Console.WriteLine(arg);
                }

                int numberOfFiles = FTPClient.Client.clientObject.UploadFiles(args, destination);
                return numberOfFiles + " uploaded.";
            }

            catch (Exception e)
            {
                return "Server not connected or Failed with exception" + e;
            }
        }
        public static string downloadMultiple( string destination, string files)
        {
            try
            {
                List<string> args = FTPClient.Console.Console.SeparateArguments(files);

                foreach (var arg in args)
                {
                    System.Console.WriteLine(arg);
                }

                int numberOfFiles = FTPClient.Client.clientObject.DownloadFiles(destination, args);
                return numberOfFiles + " downloaded.";
            }
            catch(Exception e)
            {
                return "Server not connected or Failed with exception" + e;
            }
        }
    }
}