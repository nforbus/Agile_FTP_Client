using System;
using System.Net;
using System.Security.Cryptography;
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

        public static string listRemote()
        {
            string returnMessage = "";
            string res = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing("/"))
                {
                    res += item.FullName + "\n";
                }
                returnMessage = res;
            }
            catch (Exception e)
            {
                returnMessage = "Listing failed with Exception";
            }
            return returnMessage;
        }

        public static string listLocal(string path)
        {
            string returnMessage = "";
            try
            {
                returnMessage = "The directories are:" + "\n";
                var directories = System.IO.Directory.GetDirectories(path);
                foreach (string item in directories)
                {
                    returnMessage = returnMessage + '\n' + item;
                }
            }
            catch (Exception e)
            {
                returnMessage = "Listing of local directories failed with exception";
            }
            return returnMessage;
        }

        public static string rnRemote(string target, string newName)
        {
            string returnMessage = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing("/"))
                {
                    if (item.FullName == target)
                    {
                        Client.clientObject.Rename(target, newName);
                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = "File could not be renamed.";
            }
            return returnMessage;
        }

        /*
        public static string showPerms(string path)
        {
            string returnMessage = "";
            int permissions;
            try
            {
                permissions = Client.clientObject.GetChmod(path);
                Console.Console.WriteToConsole("permissions are: " + permissions);
            }
            catch (Exception e)
            {
                returnMessage = "Couldnt check the permissions.";
            }
            return returnMessage;
        }
        */
    }
}