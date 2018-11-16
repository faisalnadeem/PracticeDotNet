using Renci.SshNet;
using System;
using System.IO;

namespace ThreadDemo
{
    public class SftpTest
    {
        public void Test()
        {
            const string host = "127.0.0.1";
            const string username = "test";
            const string password = "password";
            const string workingdirectory = "/highway/hell";
            const string uploadfile = @"c:\yourfilegoeshere.txt";

            Console.WriteLine("Creating client and connecting");
            using (var client = new SftpClient(host, 9000, username, password))
            {
                client.Connect();
                Console.WriteLine("Connected to {0}", host);

                client.ChangeDirectory(workingdirectory);
                Console.WriteLine("Changed directory to {0}", workingdirectory);

                var listDirectory = client.ListDirectory(workingdirectory);
                Console.WriteLine("Listing directory:");
                foreach (var fi in listDirectory)
                {
                    Console.WriteLine(" - " + fi.Name);
                }

                using (var fileStream = new FileStream(uploadfile, FileMode.Open))
                {
                    Console.WriteLine("Uploading {0} ({1:N0} bytes)", uploadfile, fileStream.Length);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.UploadFile(fileStream, Path.GetFileName(uploadfile));
                }
            }
        }
    }
}
