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
<<<<<<< HEAD
<<<<<<< HEAD

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
     }
=======
        public static string listRemote()
=======
        public static string lr()
>>>>>>> Resolved merged conflicts
        {
            string returnMessage = "";
            string res = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing("/pub"))
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

        public static string find(string path)
        {
            //find takes parameter as path e.g /pub/1.docx
            string returnMessage = "";
            try
            {
                //check if file exist in path
                if (Client.clientObject.FileExists(path))
                {
                    returnMessage = "File Found";
                }
                else
                {
                    returnMessage = "File not Found";
                }
           
            }
            catch (Exception e)
            {
                returnMessage = "Listing failed with Exception";
            }
            return returnMessage;
        }
    }
>>>>>>> Resolved merged conflicts
}