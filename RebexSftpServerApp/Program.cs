using Rebex.Net;
using Rebex.Net.Servers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebexSftpServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverPort = 22;
            var privateKeyPassword = "myVerySecurePassword";
            var rsaPrivateKeyFilename = "server-private-key-rsa.ppk";
            var dsaPrivateKeyFilename = "server-private-key-dsa.ppk";
            var rootFolder = @"c:\sftptemp\";
            var userName = "tester";
            var userPassword = "password";

            var server = new FileServer();

            // add one or more users
            var user = new FileServerUser(userName, userPassword, rootFolder);
            server.Users.Add(user);

            // add RSA private key for communication encryption
            server.Keys.Add(LoadOrCreatePrivateKey(rsaPrivateKeyFilename, privateKeyPassword, useRsa: true));
            // add DSA private key for communication encryption
            server.Keys.Add(LoadOrCreatePrivateKey(dsaPrivateKeyFilename, privateKeyPassword, useRsa: false));

            // bind SFTP protocol and start the server
            server.Bind(serverPort, FileServerProtocol.Sftp);
            server.Start();

            Console.WriteLine("SFTP server started. Listening on port {0}.", serverPort);
            Console.WriteLine("Press <enter> to stop.");
            Console.ReadLine();
        }

        static SshPrivateKey LoadOrCreatePrivateKey(string filename, string password, bool useRsa)
        {
            if (!File.Exists(filename))
            {
                // generate server key if it doesn't exist yet

                SshPrivateKey key;
                if (useRsa)
                {
                    // generate a 2048-bit RSA key
                    key = SshPrivateKey.Generate(SshHostKeyAlgorithm.RSA, 2048);
                }
                else
                {
                    // generate a 1024-bit DSA key
                    key = SshPrivateKey.Generate(SshHostKeyAlgorithm.DSS, 1024);
                }
                key.Save(filename, password, SshPrivateKeyFormat.Putty);

                return key;
            }

            return new SshPrivateKey(filename, password);
        }
    }
}
