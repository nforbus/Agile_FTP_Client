using System;
using System.Net;
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

        //Create directory
        public static string mkdir(string path)
        {
            string returnMessage = "";
            try
            {
                Client.clientObject.CreateDirectory(path);
                Client.clientObject.SetFilePermissions(path, 755);
                returnMessage = "Created directory: " + path;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }

        //Delete directory 
        public static string rmdir(string path)
        {
            string returnMessage = "";
            try 
            {
                Client.clientObject.DeleteDirectory(path);
                returnMessage = "Deleted directory: " + path;
            }
            catch (Exception e)
            {
                returnMessage = e.ToString();
            }
            return returnMessage;
        }

        //Change Permissions
        public static string chmod(string path,int permission)
        {
            string returnMessage = "";
            string npath;
            try
            { 
                npath = Path.Combine(Client.clientObject.GetWorkingDirectory(), path);
                Client.clientObject.SetFilePermissions(npath, permission);
                returnMessage = "Permission of file/folder: " + path +" set to :"+ permission;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }
    }
}