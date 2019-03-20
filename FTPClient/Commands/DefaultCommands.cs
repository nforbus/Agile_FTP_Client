using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using DiffMatchPatch;
using FluentFTP;

namespace FTPClient.Commands
{
    public static class DefaultCommands
    { 
        public static string Login(string address, string username="")
        {
            string returnMessage = "";
            string password = "";
            if (username == "" | username == "anonymous")
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
                    new SavedConnection(address, username).saveConnection();
                }
            }
            catch (Exception e)
            {
                returnMessage = "Connection failed with Exception:" + e;
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
                    res += item + "\n";
                }
                returnMessage = res;
            }
            catch (Exception e)
            {
                returnMessage = "Server not connected Or Listing failed with Exception:" + e;
            }
            return returnMessage;
        }

        public static string ls()
        {
            string returnMessage = "";
            try
            {
                returnMessage = "";
                var currentdir = System.IO.Directory.GetCurrentDirectory();
                var directories = System.IO.Directory.GetDirectories(currentdir);
                var files = System.IO.Directory.GetFiles(currentdir);
                foreach (string item in directories)
                {
                    var dir = new DirectoryInfo(item);
                    returnMessage += "DIR\t" + dir.Name + "\t Modified :"+dir.LastWriteTime + '\n';
                }
                foreach (string file in files)
                {
                    var time = File.GetLastWriteTime(file);
                    var size = Path.GetFileName(file).Length;
                    returnMessage += "FILE\t"+ Path.GetFileName(file) + "\t("+size +")bytes" + "\t Modified :" + time +'\n';
                }
            }
            catch (Exception e)
            {
                returnMessage = "Listing of local directories and files failed with Exception: " + e;
            }
            return returnMessage;
        }

        //Uses absoulute path for target and name changes
        public static string mv(string target, string name)
        {
            string returnMessage = "";

            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing(Client.clientObject.GetWorkingDirectory()))
                {
                    if(item.FullName == target)
                    {
                        Client.clientObject.Rename(target, name);
                        return returnMessage;
                    }
                }
                returnMessage = "1: Could not find file/directory to rename.";
            }
            catch (Exception e)
            {
                returnMessage = "Rename failed with exception: " + e;
            }
            
            return returnMessage;
        }

        //Uses absolute path for target and name changes
        public static string mvLocal(string target, string name)
        {
            string returnMessage = "";
            try
            {
                var currentdir = System.IO.Directory.GetCurrentDirectory();
                var directories = System.IO.Directory.GetDirectories(currentdir);
                var files = System.IO.Directory.GetFiles(currentdir);

                string qualifiedTarget = currentdir + "\\" + target;
                string qualifiedName = currentdir + "\\" + name;

                foreach (string item in directories)
                {
                    if (item == qualifiedTarget)
                    {
                        Directory.Move(qualifiedTarget, qualifiedName);
                        return returnMessage;
                    }

                }
                foreach (string file in files)
                {
                    if (file == qualifiedTarget)
                    {
                        File.Move(qualifiedTarget, qualifiedName);
                        return returnMessage;
                    }
                }

                returnMessage = "1: RCould not find file/directory to rename.";
            }
            catch (Exception e)
            {
                Console.Console.WriteToConsole(e.ToString());
                returnMessage = "Rename failed with exception";
            }
            return returnMessage;
        }

        public static string upload(string source, string destination)
        {
            string returnMessage = "";
            try
            {
               if(FTPClient.Client.clientObject.UploadFile(source, destination))

                returnMessage = "Upload Successful";
               else
                    returnMessage = "Upload failed";

            }
            catch (Exception e)
            {
                returnMessage = "Upload failed exception" + e;
            }
            return returnMessage;
        }

        public static string download(string source, string destination)
        {
            string returnMessage = "";

            try
            {
               FTPClient.Client.clientObject.DownloadFile(destination, source);
                returnMessage = "Download succesful";

            }
            catch (Exception e)
            {
                returnMessage = "Download failed" + e;
            }
            return returnMessage;
        }


        public static string findl(string filename)
        {
            string returnMessage = "";
            try
            {
                foreach (string file in Directory.EnumerateFiles(System.IO.Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories))
                {
                    if (filename == Path.GetFileName(file))
                    {
                        var time = File.GetLastWriteTime(file);
                        var size = Path.GetFileName(file).Length;
                        returnMessage += "FILE\t" + Path.GetFileName(file) + "\t(" + size + ")bytes" + "\t Modified :" + time + " FilePath:" + Path.GetFullPath(file) +'\n';

                    }
                }
            }
            catch (Exception e)
            {
                returnMessage = "The File does not exist: " + e;
            }

            return returnMessage;
        }

        //find files on server 
        public static string findr(string filename)
        {
            string returnMessage = "";
            try
            {
                foreach (FtpListItem item in Client.clientObject.GetListing(Client.clientObject.GetWorkingDirectory(), FtpListOption.Recursive))
                {
                    if (filename == item.Name)
                    {
                        returnMessage += item + " FilePath:"+ item.FullName +"\n";
                    }
                }
            }
            catch (Exception e)
            {
             
                returnMessage = "Server not connected or Failed with exception" + e;
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

        //disconnect from server 
        public static string exit()
        {
            string returnMessage = "";
            try
            {
                Client.serverName = null;
                Client.clientObject = null;
                FTPClient.Console.Console.readPrompt = "FTP>";
            }
            catch (Exception e)
            {
                returnMessage = "Server not connected or Failed with exception" + e;
            }
            return returnMessage;
        }

        //diff on files on local
        public static string diffl(string file1, string file2)
        {
            string returnMessage = "";
            try
            {
                string text1 = System.IO.File.ReadAllText(@file1);
                string text2 = System.IO.File.ReadAllText(@file2);

                var dmp = DiffMatchPatchModule.Default;
                List<DiffMatchPatch.Diff> diff = dmp.DiffMain(text1, text2);

                // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
                dmp.DiffCleanupSemantic(diff);
                // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
                for (int i = 0; i < diff.Count; i++)
                {
                    returnMessage += diff[i] + " ";
                }

            }
            catch (Exception e)
            {
                returnMessage = "Server not connected or Failed with exception" + e;
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
        public static string chmod(string path, int permission)
        {
            string returnMessage = "";
            string npath;
            try
            {
                npath = Path.Combine(Client.clientObject.GetWorkingDirectory(), path);
                Client.clientObject.SetFilePermissions(npath, permission);
                returnMessage = "Permission of file/folder: " + path + " set to :" + permission;
            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }
            return returnMessage;
        }

        public static string uploadMultiple(string files, string destination)
        {
            try
            {
                List<string> args = FTPClient.Console.Console.SeparateArguments(files);



                int numberOfFiles = FTPClient.Client.clientObject.UploadFiles(args, destination);
                return numberOfFiles + " uploaded.";
            }

            catch (Exception e)
            {
                return "Server not connected or Failed with exception" + e;
            }
        }

        public static string downloadMultiple( string destination, string files)
        {
            try
            {
                List<string> args = FTPClient.Console.Console.SeparateArguments(files);

             

                int numberOfFiles = FTPClient.Client.clientObject.DownloadFiles(destination, args);
                return numberOfFiles + " downloaded.";
            }
            catch(Exception e)
            {
                return "Server not connected or Failed with exception" + e;
            }
        }

        //diff on remote files
        public static string diffr(string file1, string file2)
        {
            string returnMessage = "";
            try
            {
                string name1 = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                Client.clientObject.DownloadFile(name1, file1);
                string name2 = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                Client.clientObject.DownloadFile(name2, file2);
                
                string text1 = System.IO.File.ReadAllText(name1);
                string text2 = System.IO.File.ReadAllText(name2);

                var dmp = DiffMatchPatchModule.Default;
                List<DiffMatchPatch.Diff> diff = dmp.DiffMain(text1, text2);

                // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
                dmp.DiffCleanupSemantic(diff);
                // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
                for (int i = 0; i < diff.Count; i++)
                {
                    returnMessage += diff[i] + " ";
                }

                File.Delete(name1);
                File.Delete(name2);
            }
            catch (Exception e)
            {
                returnMessage = "Server not connected or Failed with exception" + e;
            }
            return returnMessage;
        }

        //diff on file on remote and local
        public static string diff(string file1, string file2)
        {
            string returnMessage = "";
            try
            {
                string name1 = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                Client.clientObject.DownloadFile(name1, file1);

                string text1 = System.IO.File.ReadAllText(name1);
                string text2 = System.IO.File.ReadAllText(file2);

                var dmp = DiffMatchPatchModule.Default;
                List<DiffMatchPatch.Diff> diff = dmp.DiffMain(text1, text2);

                // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
                dmp.DiffCleanupSemantic(diff);
                // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]
                for (int i = 0; i < diff.Count; i++)
                {
                    returnMessage += diff[i] + " ";
                }

                File.Delete(name1);
            }
            catch (Exception e)
            {
                returnMessage = "Server not connected or Failed with exception" + e;
            }
            return returnMessage;
        }

        //Prints history of command usage
        public static string history()
        {
            string readText = File.ReadAllText("./history.txt");
            return readText;
        }

        //Deletes file from remote server
        public static string rmr(string fileToDelete)
        {
            string returnMessage = "";
            try
            {
                if(Client.clientObject.FileExists(fileToDelete))
                {
                    Client.clientObject.DeleteFile(fileToDelete);
                    returnMessage = "Deleted file: " + fileToDelete;
                }
                else {
                    returnMessage = "1: File could not be located to delete.\n";
                }

            }
            catch (Exception e)
            {
                returnMessage = e.Message;
            }

            return returnMessage;
        }
      
        public static string copyDir(string source)
         {
            string returnMessage = "";

            try
            {
                foreach (string file in Directory.EnumerateFiles(Path.GetFullPath(source), "*.*", SearchOption.AllDirectories))
                {

                    string relativePath = Path.GetRelativePath(source, file);
                    System.Console.WriteLine("Copying  "+ file);
                    FTPClient.Client.clientObject.UploadFile(file, relativePath, createRemoteDir: true);
                }
                returnMessage = "Copy succesful";

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception " + e);
            }
             return returnMessage;
        }

        public static string help()
        {
            string returnMessage = "";
            string helpstr = "Login <ftpserver> <username> when prompted <password>                        --  To login into ftpserver using the username and password\n" +
            "cd  <directory name>                                                         --  To change directory on remote\n" +
            "pwd                                                                          --  To display path of working directory\n" +
            "ls                                                                           --  To display files and directories in current directory of local\n" +
            "lr                                                                           --  To display files and directories in current directory of remote\n" +
            "findl <filename>                                                             --  To find a file with given filename on local \n" +
            "findr <filename>                                                             --  To find a file with given filename on remote \n" +
            "mvLocal <currentfile> <newfile>                                              --  Takes newfile name and replaces currentifle with it - this is for local\n" +
            "mv <currentfile> <newfile>                                                   --  Takes newfile name and replaces currentfile with it - this is for remote\n" +
            "mkdir <path>                                                                 --  Creates a directory on the given path\n" +
            "rm <path>                                                                    --  Delete a directory on the given path\n" +
            "chmod <path> <permissions>                                                   --  Sets permissions       \n" +
            "upload <local path with filename> <remote path with filename>                --  Uploads a file from local to remote with the name as filename\n" +
            "download <local path with filename> <remote path with filename>              --  Downloads a file from remote to local with the name as filename\n" +
            "uploadMultiple <\"file1 file2 'filename with spaces' file4\"> <destination>    --  Uploads multiple files given as args on remote at the destination location provided by user\n" +
            "downloadMultiple <destination> <\"file1 file2 'filename with spaces' file4\">  --  Downloads multiple files given as args from remote, to the destination location on local\n" +
            "copyDir <source path>                                                        --  Copies the directory from the provided source path to current working directory on remote \n" +
            "diffl <filepath1> <filepath2>                                                --  Difference of files on local\n" +
            "diffr <filepath1> <filepath2>                                                --  Difference of files on remote\n" +
            "diff  <filepath1> <filepath2>                                                --  Difference of files on remote and local\n" +
            "history                                                                      --  Prints history of command lines\n" +
            "rmr <filepath>                                                               --  Removes file at filepath location\n" +
            "exit                                                                         --  To disconnect from server\n";
            try
            {
                System.Console.WriteLine(@helpstr);
            }
            catch (Exception e)
            {
                returnMessage = e.ToString();
            }
            return returnMessage;
        }

        public static string saved()
        {
            List<SavedConnection> savedConns = SavedConnection.getSavedConnections();

            if (savedConns.Count > 0)
            {
                int counter = 1;
                foreach (var connection in savedConns)
                {
                    System.Console.WriteLine("[" + counter + "] " + connection.ToString());
                    counter += 1;
                }

                System.Console.Write("Enter the saved connection you want to use: ");
                int index = Convert.ToInt32(System.Console.ReadLine());
                System.Console.WriteLine("Logging into: " + savedConns[index-1]);
                return Login(savedConns[index - 1].address, savedConns[index - 1].username);
            }
            else
            {
                return "There is no saved information. Please use the Login command to connect to a server.";
            }
        }

        public static string runTests()
        {
            /* Sets up the unit testing environment;
             * Keeps an index of the number of success & fail tests
             * Keeps an index of the number of success & fail tests that behave appropriately
             * Creates a connection if there isn't one (prompts password for the ftpuser)
             * Creates the unittests directory and goes into it
             * Creates a test file inside of it
             * Deletes the entire unittests directory at the end of the test, and logs out. */

            string returnMessage = null;
            int successfulSuccess = 0;
            int successfulFail = 0;
            int actualSuccess = 0;
            int actualFail = 0;
            string testSuccess = null;
            string testFail = null;

            if(Client.clientObject == null)
            {
                Login("35.185.209.33", "ftpuser");
            }

            if(Client.clientObject.DirectoryExists("/unittests"))
            {
                cd("/unittests");
            }
            else
            {
                mkdir("/unittests");
                cd("/unittests");
            }

            FileStream writeTestFile;
            StreamWriter writer = null;
            writeTestFile = new FileStream("./test.txt", FileMode.Append, FileAccess.Write);
            writer = new StreamWriter(writeTestFile);
            writer.WriteLine("Test.\n");
            writer.Close();
            writer.Dispose();

            upload("./test.txt", "./test.txt");

            //Add tests below for each of the functions you want to unit test

            //Test mv()
            successfulSuccess += 1;
            successfulFail += 1;

            testSuccess = mv("/test", "/test2");
            mv("/test2", "/test");
            testFail = mv("/test2", "/test3");

            if(!testSuccess[0].Equals("1"))
            {
                ++actualSuccess;
            }

            if(!testFail[0].Equals("1"))
            {
                ++actualFail;
            }

            testSuccess = null;
            testFail = null;

            //Test mvLocal()
            successfulSuccess += 1;
            successfulFail += 1;

            File.Create("./test.txt");
            testSuccess = mvLocal("./test.txt", "./test2.txt");
            mvLocal("./test2.txt", "./test.txt");
            testFail = mvLocal("./test2.txt", "./test3.txt");

            if (!testSuccess[0].Equals("1"))
            {
                ++actualSuccess;
            }

            if (!testFail[0].Equals("1"))
            {
                ++actualFail;
            }

            testSuccess = null;
            testFail = null;

            //Test history() -- does not need a test

            //Test rmr()
            successfulSuccess += 1;
            successfulFail += 1;

            testSuccess = rmr("./test.txt");
            upload("./test.txt", "./test.txt");
            testFail = rmr("./test2.txt");

            if (!testSuccess[0].Equals("1"))
            {
                ++actualSuccess;
            }

            if (!testFail[0].Equals("1"))
            {
                ++actualFail;
            }

            testSuccess = null;
            testFail = null;

            //cleanup and return results
            returnMessage = "Out of " + successfulSuccess + " success tests, " + actualSuccess + " tests were successful as expected.\n";
            returnMessage += "Out of " + successfulFail + " failure tests, " + actualFail + " tests failed as expected.\n";

            rmdir("/unittests");
            exit();

            return returnMessage;
        }
    }
}