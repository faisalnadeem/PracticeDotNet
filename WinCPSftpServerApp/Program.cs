using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace WinCPSftpServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string logname = "log.xml";

            // Run hidden WinSCP process
            Process winscp = new Process();
            winscp.StartInfo.FileName = "winscp";
            winscp.StartInfo.Arguments = "/xmllog=\"" + logname + "\"";
            winscp.StartInfo.UseShellExecute = false;
            winscp.StartInfo.RedirectStandardInput = true;
            winscp.StartInfo.RedirectStandardOutput = true;
            winscp.StartInfo.CreateNoWindow = true;
            winscp.Start();

            // Feed in the scripting commands
            winscp.StandardInput.WriteLine("option batch abort");
            winscp.StandardInput.WriteLine("option confirm off");
            winscp.StandardInput.WriteLine("open mysession");
            winscp.StandardInput.WriteLine("ls");
            winscp.StandardInput.WriteLine("put d:\\examplefile.txt");
            winscp.StandardInput.Close();

            // Collect all output (not used in this example)
            string output = winscp.StandardOutput.ReadToEnd();

            // Wait until WinSCP finishes
            winscp.WaitForExit();

            // Parse and interpret the XML log
            // (Note that in case of fatal failure the log file may not exist at all)
            XPathDocument log = new XPathDocument(logname);
            XmlNamespaceManager ns = new XmlNamespaceManager(new NameTable());
            ns.AddNamespace("w", "http://winscp.net/schema/session/1.0");
            XPathNavigator nav = log.CreateNavigator();

            // Success (0) or error?
            if (winscp.ExitCode != 0)
            {
                Console.WriteLine("Error occured");

                // See if there are any messages associated with the error
                foreach (XPathNavigator message in nav.Select("//w:message", ns))
                {
                    Console.WriteLine(message.Value);
                }
            }
            else
            {
                // It can be worth looking for directory listing even in case of
                // error as possibly only upload may fail

                XPathNodeIterator files = nav.Select("//w:file", ns);
                Console.WriteLine("There are {0} files and subdirectories:", files.Count);
                foreach (XPathNavigator file in files)
                {
                    Console.WriteLine(file.SelectSingleNode("w:filename/@value", ns).Value);
                }
            }
        }
    }
}
