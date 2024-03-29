﻿using FishSellingOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            
            _logger = logger;
        }

        public IActionResult Index()
        {
            CreateContainer();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
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

        public void CreateContainer()
        {
            CloudBlobContainer container = getBlobContainerInformation();

            container.CreateIfNotExistsAsync();


        }
    }
}
