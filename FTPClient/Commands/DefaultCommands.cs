using System;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.IO;
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
                    res += item.Name + "\n";
                }
                returnMessage = res;
            }
            catch (Exception e)
            {
                returnMessage = "Listing failed with Exception";
            }
            return returnMessage;
        }

        public static string ls()
        {
            string returnMessage = "";
            try
            {
                returnMessage = "The directories and files are:" + "\n";
                var currentdir = System.IO.Directory.GetCurrentDirectory();
                var directories = System.IO.Directory.GetDirectories(currentdir);
                var files = System.IO.Directory.GetFiles(currentdir);
                foreach (string item in directories)
                {
                    var dir = new DirectoryInfo(item).Name;
                    returnMessage = returnMessage + '\n' + dir;
                }
                foreach (string file in files)
                {
                    returnMessage = returnMessage + '\n' + Path.GetFileName(file);
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
    }
}