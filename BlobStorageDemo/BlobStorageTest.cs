using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BlobStorageDemo
{
    public class BlobStorageTest
    {
        private static readonly Fixture Fixture = new Fixture();
        public static void Test()
        {
            var shipmentTrackingEvents =  Fixture.Build<ShipmentTrackingEvent>()
                .CreateMany<ShipmentTrackingEvent>(10).ToList(); 
                
            var jsonString = ConvertToJsonString(shipmentTrackingEvents);
            BlobResponse blobResponse;
            //using (var fileContent = GenerateStreamFromString(jsonString))
            //{
            //    string filePathName = "";
            //    blobResponse = await _blobStorageClient.Store(AzureStorageConstants.TRACKING_FILES_CONTAINER_NAME,
            //        new Blob(fileContent, filePathName));
            //}

            //// Create a BlobServiceClient object which will be used to create a container client
            //BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            ////Create a unique name for the container
            //string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            //// Create the container and return a container client object
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        }

        public static void EnumerateBlobs()
        {
            //const string connectionStringQa = "DefaultEndpointsProtocol=https;AccountName=sortedcoreqa;AccountKey=6MAT3kKjyDgSnVEOgnEL4RfldGMfB/wYLeZBoA27uiMoTthWABzKNiE3Ku7OcfUNVJwkgJEWSK0qnTaY+NGD/A==;BlobEndpoint=https://sortedcoreqa.blob.core.windows.net/;QueueEndpoint=https://sortedcoreqa.queue.core.windows.net/;TableEndpoint=https://sortedcoreqa.table.core.windows.net/;FileEndpoint=https://sortedcoreqa.file.core.windows.net/;";
            const string connectionStringDev = "DefaultEndpointsProtocol=https;AccountName=test12337458597;AccountKey=6P12zP+n4sEYN3UUxQKbhCuLare4boyI++7IQNgyqo/cXjZxfkoXkR8DN8epB5R+8hNX//nB9bHfZBacDFg2hw==;BlobEndpoint=https://test12337458597.blob.core.windows.net/;QueueEndpoint=https://test12337458597.queue.core.windows.net/;TableEndpoint=https://test12337458597.table.core.windows.net/;FileEndpoint=https://test12337458597.file.core.windows.net/;";
            //const string connectionStringLocal = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            const string containerName = "test-local";
            const string filePath = "c:\\temp\\test.txt";

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionStringDev, containerName);
            //container.Create();

            // Upload a few blobs so we have something to list
            using FileStream fileToUpload = File.OpenRead(filePath);
            container.UploadBlob("test-first", fileToUpload);
            fileToUpload.Close();
            //container.UploadBlobAsync("second", File.OpenRead(filePath));
            //container.UploadBlobAsync("third", File.OpenRead(filePath));

            // Print out all the blob names
            foreach (BlobItem blob in container.GetBlobs())
            {
                Console.WriteLine(blob.Name);
            }
        }

        



        private static string ConvertToJsonString(List<ShipmentTrackingEvent> shipmentTrackingEvents)
        {
            return JsonConvert.SerializeObject(shipmentTrackingEvents, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });
        }
        private static Stream GenerateStreamFromString(string jsonString)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}