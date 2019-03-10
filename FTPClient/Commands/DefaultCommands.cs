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

        //create directory
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

        //delete directory 
        public static string rm(string path)
        {
            string returnMessage = "";
            try 
            {
                Client.clientObject.DeleteDirectory(path);
                returnMessage = "Deleted directory" + path;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }

        public static string setPermission(string path,int permission)
        {
            string returnMessage = "";
            try
            { 
                Client.clientObject.SetFilePermissions(path, permission);
                returnMessage = "Permission of file/folder: " + path +" set to :"+ permission;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }

        //resume file transfer
        public static string resumeFileTransfer(string path,string rpath)
        {
            string returnMessage = "";
            try
            {
                Client.clientObject.UploadFile(path,rpath, FtpExists.Append);
                returnMessage = "Complete Transfer: File uploaded to: " + rpath;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }
    }
}