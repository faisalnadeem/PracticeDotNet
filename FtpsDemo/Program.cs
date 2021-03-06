﻿using System;
using System.IO;
using System.Net;

namespace FtpsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ListFilesOnServerSsl( new Uri("ftp://mft2.svc.netdespatch.com"));
            Console.WriteLine("Hello World!");
        }


        public static bool ListFilesOnServerSsl(Uri serverUri)
        {
            // The serverUri should start with the ftp:// scheme.
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.EnableSsl = true;

            // Get the ServicePoint object used for this request, and limit it to one connection.
            // In a real-world application you might use the default number of connections (2),
            // or select a value that works best for your application.

            ServicePoint sp = request.ServicePoint;
            Console.WriteLine("ServicePoint connections = {0}.", sp.ConnectionLimit);
            sp.ConnectionLimit = 1;

            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            Console.WriteLine("The content length is {0}", response.ContentLength);
            // The following streams are used to read the data returned from the server.
            Stream responseStream = null;
            StreamReader readStream = null;
            try
            {
                responseStream = response.GetResponseStream();
                readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);

                if (readStream != null)
                {
                    // Display the data received from the server.
                    Console.WriteLine(readStream.ReadToEnd());
                }

                Console.WriteLine("List status: {0}", response.StatusDescription);
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
            }

            Console.WriteLine("Banner message: {0}",
                response.BannerMessage);

            Console.WriteLine("Welcome message: {0}",
                response.WelcomeMessage);

            Console.WriteLine("Exit message: {0}",
                response.ExitMessage);
            return true;
        }
    }
}


