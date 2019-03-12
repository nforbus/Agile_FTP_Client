using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net;

namespace FTPClient
{
    public class SavedConnection
    {
        public string address { get; private set; } = "";
        public string username { get; private set; } = "";
        
        private static string fileName = "./saved.txt";

        public SavedConnection(string address, string username)
        {
            this.address = address;
            this.username = username;
        }

        public void saveConnection()
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "");
            }
            
            bool unique = true;
            foreach (var connection in getSavedConnections())
            {
                if (connection.ToString() == this.ToString())
                {
                    unique = false;
                    break;
                }
            }

            if (!unique) return;
            
            StreamWriter writer = new StreamWriter(File.Open(fileName, System.IO.FileMode.Append))
            {
                AutoFlush = true
            };   
                
            writer.WriteLine("{0},{1}", address, username);
            writer.Close();
            return;
        }

        public static List<SavedConnection> getSavedConnections()
        {
            List<SavedConnection> conns = new List<SavedConnection>();

            if (File.Exists(fileName))
            {
                foreach (var line in File.ReadAllLines(fileName))
                {
                    string[] fields = line.Split(",");
                    if (fields != null && fields.Length == 2)
                    {
                        SavedConnection temp = new SavedConnection(fields[0], fields[1]);
                        conns.Add(temp);
                        
                    }
                }
            }

            return conns;
        }
        
        public override string ToString()
        {
            return "Server: " + address + " | Username: " + username;
        }
    }
}