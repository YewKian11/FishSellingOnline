using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Controllers
{
    public class BlobsController : Controller
    {
        private CloudBlockBlob blob;

        private CloudBlobContainer getBlobContainerInformation()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();

            CloudStorageAccount objectaccount =
            CloudStorageAccount.Parse(configure["ConnectionStrings:fishsellingonlineconnection"]);

            CloudBlobClient blobclient = objectaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference("fishsellingonlineblob");
            return container;
        }

        public IActionResult CreateContainer()
        {
            CloudBlobContainer container = getBlobContainerInformation();

            ViewBag.result = container.CreateIfNotExistsAsync().Result;

            ViewBag.ContainerName = container.Name;

            return View();
        }
        // Gallery 
        public ActionResult ListItemGallery(string Message = null) {

            ViewBag.msg = Message;
            //get container information
            CloudBlobContainer container = getBlobContainerInformation();

            // create list, store blobs information
            List<string> blobs = new List<string>();

            //Find Blob Result
            BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;

            //track blob
            foreach (IListBlobItem item in result.Results) 
            {
                //blob type identify 
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    if (Path.GetExtension(blob.Name.ToString()) == ".jpg" ||
                        Path.GetExtension(blob.Name.ToString()) == ".png") { 
                    blobs.Add(blob.Name + "#" + blob.Uri.ToString());
                    }
                    
                } 
                else if(item.GetType() ==typeof(CloudPageBlob)) 
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name + "#" + blob.Uri.ToString());
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory blob = (CloudBlobDirectory)item;
                    blobs.Add(blob.Uri.ToString());
                }
            }
            return View(blobs);
        }

        //Download Blob refer, asp-action 
        public async Task<ActionResult> downloadblobAsync(string imagename,string urlimage) {
            CloudBlobContainer container = getBlobContainerInformation();
            string message = null;
          
            try
            {
                // CloudBlockBlob item = container.GetBlockBlobReference(imagename);
                // var outputitem = System.IO.File.OpenWrite(@"C:\\Users\\\Kong\\Downloads\\"+imagename);
                //item.DownloadToStreamAsync(outputitem).Wait();
                //message = imagename + " is downloaded successfully" ;
                await using (MemoryStream ms = new MemoryStream())
                {
                    blob = container.GetBlockBlobReference(imagename);
                    await blob.DownloadToStreamAsync(ms);
                }
                Stream blobstream = blob.OpenReadAsync().Result;
                return File(blobstream, blob.Properties.ContentType, blob.Name);
              //  outputitem.Close();
            }
            catch (Exception ex)
            {
                message = "Unable to download file " + imagename +
                        "\\n Technical issues:" + ex.ToString() + "Please try dowload file again";
            }
            return RedirectToAction("ListItemGallery", "Blobs", new { Message = message });
        }



        //Delete Blob refer, asp-action
        public ActionResult deleteblob(string area)
        {
            CloudBlobContainer container = getBlobContainerInformation();
            string message = "";


            try 
            {
                CloudBlockBlob item = container.GetBlockBlobReference(area);
                message = item.Name + " is deleted";
                item.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                message = "Unable to download file " + area +
                        "\\n Technical issues:" + ex.ToString() + "Please try dowload file again";
            }

            return RedirectToAction("ListItemGallery", "Blobs", new { Message = message});

        }
    }
}
