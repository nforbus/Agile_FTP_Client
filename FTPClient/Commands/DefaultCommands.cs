using System;

namespace FTPClient.Commands
{
    public static class DefaultCommands
    {
        public static string DoSomething(int id, string data)
        {
            return string.Format(
                "I did something to the record Id {0} and save the data {1}", id, data);
        }
    }
}