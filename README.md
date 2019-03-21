# Agile_FTP_Client
An FTP client developed for CS410 - Agile group project


`Login <ftpserver> <username> when prompted <password>`     --  To login into ftpserver using the username and password
  
`cd  <directory name>`                                      --  To change directory on remote
  
`pwd`                                                       --  To display path of working directory

`ls`                                                        --  To display files and directories in current directory of local

`lr`                                                        --  To display files and directories in current directory of remote

`findl <filename>`                                          --  To find a file with given filename on local 

`findr <filename>`                                          --  To find a file with given filename on remote 

`mvLocal <currentfile> <newfile>`                           --  Takes newfile name and replaces currentifle with it - this is for local

`mv <currentfile> <newfile>`                                --  Takes newfile name and replaces currentfile with it - this is for remote

`mkdir <path>`                                              --  Creates a directory on the given path

`rm <path>`                                                 --  Delete a directory on the given path

`chmod <path> <permissions>`                                --  Sets permissions       

`upload <local path with filename> <remote path with filename>`--  Uploads a file from local to remote with the name as filename

`download <local path with filename> <remote path with filename>`--  Downloads a file from remote to local with the name as filename

`uploadMultiple <"file1 file2 'filename with spaces' file4"> <destination>`--  Uploads multiple files given as args on remote at the destination location provided by user

`downloadMultiple <destination> <"file1 file2 'filename with spaces' file4">`--  Downloads multiple files given as args from remote, to the destination location on local

`copyDir <source path>`                                          --  Copies the directory from the provided source path to current working directory on remote 

`diffl <filepath1> <filepath2>`                                  --  Difference of files on local

`diffr <filepath1> <filepath2>`                                  --  Difference of files on remote

`diff  <filepath1> <filepath2>`                                  --  Difference of files on remote and local

`history`                                                        --  Prints history of command lines

`rmr <filepath>`                                                 --  Removes file at filepath location

`exit`                                                           --  To disconnect from server

