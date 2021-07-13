using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStoragteMvcCore31.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BlobStoragteMvcCore31.Controllers
{
    public class DemoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _client;
        const string connectionStringDevPasLogic = "DefaultEndpointsProtocol=https;AccountName=propertyfiles4demo;AccountKey=ere+Y+XDkFb5O+or7DPvXSTbmZXUViTqV8QL0R+GfcBaZAY2RAt16m4GSNbs7HDHY6YUwFD0srHiUsVX64vV/A==;EndpointSuffix=core.windows.net";
            const string containerName = "property-files";
        public DemoController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new BlobContainerClient(connectionStringDevPasLogic, containerName);
            _client.CreateIfNotExists();
        }

        public async Task<IActionResult> Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile files)
        {
            string systemFileName = files.FileName;
            //string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            // Retrieve storage account from connection string.
            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            //CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobClient = new BlobContainerClient(connectionStringDevPasLogic, containerName);

            //// Retrieve a reference to a container.
            //CloudBlobContainer container = blobClient.GetContainerReference("filescontainers");
            //// This also does not make a service call; it only creates a local object.
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);
            var blockBlob = await blobClient.CreateIfNotExistsAsync();

            await using (var data = files.OpenReadStream())
            {
                await blobClient.UploadBlobAsync(systemFileName, data); //blockBlob.UploadFromStreamAsync(data);
            }
            return View("Create");
        }

        public List<FileData> GetAll()
        {
            var blobClient = new BlobContainerClient(connectionStringDevPasLogic, containerName);

            return blobClient.GetBlobs().Select(blob => new FileData
            {
                FileName = blob.Name,
                FileSize = Math.Round((Convert.ToSingle(blob.Properties.ContentLength) / 1024f) / 1024f, 2)
                    .ToString(CultureInfo.InvariantCulture),
                ModifiedOn = DateTime.Parse(blob.Properties.LastModified.ToString()).ToLocalTime()
                    .ToString(CultureInfo.InvariantCulture)
            }).ToList();

        }
        public async Task<IActionResult> ShowAll()
        {
            return View(GetAll());
        }

        public async Task<string> DownloadAsString(string pathAndFileName)
        {
            BlobClient blobClient = _client.GetBlobClient(pathAndFileName);
            if (await blobClient.ExistsAsync())
            {
                BlobDownloadInfo download = await blobClient.DownloadAsync();
                byte[] result = new byte[download.ContentLength];
                await download.Content.ReadAsync(result, 0, (int)download.ContentLength);
 
                return Encoding.UTF8.GetString(result);
            }
            return string.Empty;
        }
 
        /// <summary>
        /// Delete file in container
        /// </summary>
        /// <param name="pathAndFileName">Full path to the container file</param>
        /// <returns>True if file was deleted</returns>
        //public async Task<bool> Delete(string pathAndFileName)
        //{
        //    BlobClient blobClient = _client.GetBlobClient(pathAndFileName);
        //    return await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        //}

        public async Task<IActionResult> Download(string blobName)
        {
            BlobClient blobClient = _client.GetBlobClient(blobName); 
            if (!await blobClient.ExistsAsync()) return null;

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return File(download.Content, download.ContentType, blobName);
        }
        
        public async Task<IActionResult> Delete(string blobName)
        {
            var blobClient = _client.GetBlobClient(blobName); 
            if (!await blobClient.ExistsAsync()) return null;

            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            return RedirectToAction("ShowAll", "Demo");
        }

        public static void ListBlobs()
        {
            //const string connectionStringQa = "DefaultEndpointsProtocol=https;AccountName=sortedcoreqa;AccountKey=6MAT3kKjyDgSnVEOgnEL4RfldGMfB/wYLeZBoA27uiMoTthWABzKNiE3Ku7OcfUNVJwkgJEWSK0qnTaY+NGD/A==;BlobEndpoint=https://sortedcoreqa.blob.core.windows.net/;QueueEndpoint=https://sortedcoreqa.queue.core.windows.net/;TableEndpoint=https://sortedcoreqa.table.core.windows.net/;FileEndpoint=https://sortedcoreqa.file.core.windows.net/;";
            //const string connectionStringDev = "DefaultEndpointsProtocol=https;AccountName=test12337458597;AccountKey=6P12zP+n4sEYN3UUxQKbhCuLare4boyI++7IQNgyqo/cXjZxfkoXkR8DN8epB5R+8hNX//nB9bHfZBacDFg2hw==;BlobEndpoint=https://test12337458597.blob.core.windows.net/;QueueEndpoint=https://test12337458597.queue.core.windows.net/;TableEndpoint=https://test12337458597.table.core.windows.net/;FileEndpoint=https://test12337458597.file.core.windows.net/;";
            //const string connectionStringLocal = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            const string filePath = "c:\\temp\\Risk.pdf";

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionStringDevPasLogic, containerName);
            container.Create();

            // Upload a few blobs so we have something to list
            using FileStream fileToUpload = System.IO.File.OpenRead(filePath);
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
    }
}