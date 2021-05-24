using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml;
using AutoFixture;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BlobStorageDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            var randomString = RandomStringGenerator.RandomString(48);
            BlobStorageTest.EnumerateBlobs();
            Console.WriteLine("Hello World!");
        }
    }

    public class RandomStringGenerator
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

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



    public class ShipmentTrackingEvent
    {
        public ShipmentSummary Shipment { get; set; }

        public TrackingEvent[] Events { get; set; }
    }
    public class ShipmentSummary
    {
        public string Id { get; set; }


        public bool MayBeMissing { get; set; }

        public string Retailer { get; set; }

        public string CustomerId { get; set; }

        public string Carrier { get; set; }

        public string CarrierReference { get; set; }

        public string PackageTrackingReference { get; set; }

        public string CarrierIntegrationId { get; set; }

        public string[] CustomReferences { get; set; } = { };

        public string[] TrackingReferences { get; set; } = { };

        public string[] Tags { get; set; }

        public DateTimeOffset? DeliveredDate { get; set; }

        public DateTimeOffset? LastTracked { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public bool IsTrackable { get; set; }

        public string[] CarrierTrackingUrls { get; set; }

        public string ShippingLocationReference { get; set; }

        public string TenantReference { get; set; }

        public string ChannelReference { get; set; }
    }
    public class TrackingEvent
    {
        public string Id { get; set; }

        public string CarrierIntegrationId { get; set; }

        public string CarrierId { get; set; }

        public string SortedCarrierName { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset ReceivedTimestamp { get; set; }

        public string EventCode { get; set; }

        public int EventPriority { get; set; }

        public string RawEvent { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string ShipmentId { get; set; }

        public string CustomerId { get; set; }

        public string CorrelationId { get; set; }

        public string EventSignature { get; set; }

        public string TrackingReference { get; set; }

        public string PackageTrackingReference { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

    }
    public class Blob
    {
        public Blob(Stream content, string reference)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        public Stream Content { get; }

        public string Reference { get; }

    }
    public class BlobResponse
    {
        public BlobResponse(Uri uri, string saSToken, DateTimeOffset tokenExpiry)
        {
            Uri = uri;
            SaSToken = saSToken;
            TokenExpires = tokenExpiry;
        }
        public Uri Uri { get; }

        public string SaSToken { get; }

        public DateTimeOffset TokenExpires { get; }

        public override string ToString()
        {
            return string.Concat(Uri, SaSToken);
        }
    }

}
