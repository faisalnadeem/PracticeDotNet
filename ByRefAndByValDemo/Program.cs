using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ByRefAndByValDemo
{
    class Program
    {
        private const int MAX_FILE_SIZE = 125829120; //120 megabytes
        private const string ORIGINAL_FILE_PATH = "original-file-path";
        private const string TRACKING_CONFIGURATION_ID = "tracking-configuration-id";


        static async Task Main(string[] args)
        {
            var remoteFiles = (await ftpClientListFiles()).ToArray();
            Console.WriteLine($"ConfigurationId \"messageConfigurationId\" Connected to configuration.Host. {remoteFiles.Length} file(s) to process.");
            var configurationRemotePath = "folder/";

            var metadata = new Dictionary<string, string>();
            foreach (var remoteFile in remoteFiles)
            {
                if (remoteFile.FileSize > MAX_FILE_SIZE)
                {
                    Console.WriteLine($"File {remoteFile.FileName} ({remoteFile.FileSize} bytes) is too large to download ");
                    continue;
                }

                var messageConfigurationId = remoteFile.FileSize.ToString();
                Console.WriteLine($"Remote file name is  : {remoteFile.FileName}");
                var originalFilePath = Path.Combine(configurationRemotePath, remoteFile.FileName);
                Console.WriteLine($"original file path is : {originalFilePath}");
                MapCustomProperties(metadata, messageConfigurationId, originalFilePath);

                await TrackingFileSender_SendTrackingFile(remoteFile.FileSize, remoteFile.FileName, "configuration.DownloadTarget", "message.CustomerId", metadata);


            }


            Console.WriteLine("Press any key to exit....");
            Console.ReadLine();
        }

        private static async Task TrackingFileSender_SendTrackingFile(long remoteFileFileSize, string remoteFileFileName, object downloadTarget, object customerId, Dictionary<string, string> metadata)
        {
            Console.WriteLine($"Values for configuration id  - metadata first key: {metadata.First().Key} metadata first value: {metadata.First().Value}");
            Console.WriteLine($"Values for original file path- metadata first key: {metadata.Last().Key} metadata first value: {metadata.Last().Value}");
        }

        private static void MapCustomProperties(IDictionary<string, string> metadata, string configurationId, string originalFilePath)
        {
            if (!metadata.ContainsKey(TRACKING_CONFIGURATION_ID))
                metadata.Add(TRACKING_CONFIGURATION_ID, configurationId);
            else
                metadata[TRACKING_CONFIGURATION_ID] = configurationId;

            if (!metadata.ContainsKey(ORIGINAL_FILE_PATH))
                metadata.Add(ORIGINAL_FILE_PATH, originalFilePath);
            else
            {
                metadata[ORIGINAL_FILE_PATH] = originalFilePath;
            }
        }


        private static async Task<List<RemoteFileSummary>> ftpClientListFiles()
        {
            var listFiles = new List<RemoteFileSummary>();

            for (var i = 0; i < 500; i++)
            {
                listFiles.Add(new RemoteFileSummary($"filename-{i}", 12 * i));
            }

            return listFiles;
        }
    }

    public class RemoteFileSummary
    {
        public RemoteFileSummary(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
        public string FileName { get; set; }
        public long FileSize { get; }
    }
}
