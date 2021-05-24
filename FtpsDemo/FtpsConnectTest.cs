using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FtpsDemo
{
    class FtpsConnectTest
    {
        class FTP
        {
            private string host = null;
            private string user = null;
            private string pass = null;
            private FtpWebRequest ftpRequest = null;
            private FtpWebResponse ftpResponse = null;
            private Stream ftpStream = null;
            private int bufferSize = 2048;

            /* Construct Object */
            public FTP(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

            /* Download File */
            public void upload(string remoteFile, string localFile)
            {
                try
                {
                    /* Create an FTP Request */
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                    /* Log in to the FTP Server with the User Name and Password Provided */
                    ftpRequest.Credentials = new NetworkCredential(user, pass);
                    /* When in doubt, use these options */
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.EnableSsl = true;
                    ftpRequest.UsePassive = true;
                    /* Specify the Type of FTP Request */
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    /* Establish Return Communication with the FTP Server */
                    ftpStream = ftpRequest.GetRequestStream();
                    /* Open a File Stream to Read the File for Upload */
                    FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                    /* Buffer for the Downloaded Data */
                    byte[] byteBuffer = new byte[bufferSize];
                    int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                    try
                    {
                        while (bytesSent != 0)
                        {
                            ftpStream.Write(byteBuffer, 0, bytesSent);
                            bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    /* Resource Cleanup */
                    localFileStream.Close();
                    ftpStream.Close();
                    ftpRequest = null;
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); Console.Read(); }
                return;
            }
        }
    }
}
