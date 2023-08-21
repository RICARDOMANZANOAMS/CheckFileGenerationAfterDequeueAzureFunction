using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ricardo
{
    public static class testtrigger
    {
        [FunctionName("testtrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "RequestStatus/{thisGUID}")] HttpRequest req,string thisGUID,
            ILogger log)
        {
            
            string connectionString = "Paste connection string for storage"; //Connection string
            string containerName = "Paste container name"; //container name data
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);//Connect to the storage
        
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName); //Connect to the blob
           
           
            string name_complete=thisGUID+".txt";  //Generate name
            Console.WriteLine(name_complete);

            BlobClient blobClient = containerClient.GetBlobClient(name_complete);  //Create blobclient
            bool exists = await blobClient.ExistsAsync();   //Verify if file name as guid.txt exists in the blob
            Console.WriteLine(exists);
            if (exists==false){          //If the file does not exit, it means that the processing is not done yet
                 return new OkObjectResult(new { status = "Not found"});          
            }           
            else{  //If the file is written in blob, the function verifies if the content of the file is correct
                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();  //Download the file
                string downloadedData = downloadResult.Content.ToString();  //Transform to string the file
                Console.WriteLine(downloadedData);
                if (downloadedData=="API error"){   //Verify if the info is in error or successful
                    return new OkObjectResult(new { status = "Api problem"});

                }
                else{
                    return new OkObjectResult(new { status = "Successful"});

                }
                
            }

            
           
            
        }
    }
}
