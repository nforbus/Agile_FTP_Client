using System;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using FluentFTP;

namespace FTPClient.Commands
{
    public static class DefaultCommands
    {

        public static string Login(string address, string username = "")
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

                    returnMessage = "Connected to " + address;
                }
            }
            catch (Exception e)
            {
                returnMessage = "Connection failed with Exception";
            }

            return returnMessage;
        }

        public static string ls(string path)
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
                returnMessage = "Listing of local directories failed with Exception";
            }
            return returnMessage;
        }

    }
}
    
