using System;
using FluentFTP;

namespace FTPClient.Commands
{
    public static class DefaultCommands
    {

        public static string Login(string address)
        {
            string returnMessage = "";
            try
            {
                FtpClient client = new FtpClient(address);

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

        public static string listLocalDir(string path)
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
    
