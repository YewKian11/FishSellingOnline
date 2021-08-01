using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishSellingOnline.Data;
using FishSellingOnline.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Storage;

namespace FishSellingOnline.Views
{
    public class ProductsController : Controller
    {
        private readonly FishSellingOnlineProductContext _context;

        public ProductsController(FishSellingOnlineProductContext context)
        {

            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductImage,ProductDescription,ProductPrice,stockleft")] Product product, List<IFormFile> ProductImage)
        {
            if (ModelState.IsValid)
            {
                CloudBlobContainer container = getBlobContainerInformation();
                CloudBlockBlob blobitem = null;
                foreach (var item in ProductImage)
                {
                    if (item.Length > 0)
                    {
                        blobitem = container.GetBlockBlobReference(item.FileName);
                        var blobstream = item.OpenReadStream();
                        blobitem.UploadFromStreamAsync(blobstream).Wait();
                        using (var stream = new MemoryStream())
                        {

                            await item.CopyToAsync(stream);
                            product.ProductImage = stream.ToArray();
                        }
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductImage,ProductDescription,ProductPrice,stockleft")] Product product, List<IFormFile> ProductImage)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CloudBlobContainer container = getBlobContainerInformation();
                    CloudBlockBlob blobitem = null;
                    foreach (var item in ProductImage)
                    {
                        if (item.Length > 0)
                        {
                            blobitem = container.GetBlockBlobReference(item.FileName);
                            var blobstream = item.OpenReadStream();
                            blobitem.UploadFromStreamAsync(blobstream).Wait();
                            using (var stream = new MemoryStream())
                            {
                                await item.CopyToAsync(stream);
                                product.ProductImage = stream.ToArray();
                            }
                        }
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
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
    }
}
