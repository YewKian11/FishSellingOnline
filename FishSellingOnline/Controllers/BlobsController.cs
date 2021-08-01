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
    }
}
