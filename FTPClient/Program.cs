using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FTPClient
{
    class Program
    {
        const string _commandNamespace = "FTPClient.Commands";
        static Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> _commandLibraries;

        static void Main(string[] args)
        {
            FTPClient.Console.Console.Run();
        }

    }
}