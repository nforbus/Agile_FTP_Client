using FluentFTP;

namespace FTPClient
{
    public static class Client
    {
        public static string serverName = "";
        public static FtpClient clientObject = null;
        public static bool viewingRemote = false;
    }
}