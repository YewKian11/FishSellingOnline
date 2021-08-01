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

namespace FishSellingOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly FishSellingOnlineCartContext _context;

        public CartController(FishSellingOnlineCartContext context)
        {
            _context = context;
        }

        // GET: Products/Cart
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cart.ToListAsync());
        }

        // GET: Products/Cart/Details/
        public async Task<IActionResult> CartDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(n => n.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Products/AddToCart

        public IActionResult AddToCart()
        {
            return View();
        }

        // POST: Products/AddToCart
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([Bind ("CartID,ProductID,ProductName,ProductImage,ProductDescription,ProductPrice,Quantity,Total")] Cart cart, List<IFormFile> ProductImage)
        {
            if(ModelState.IsValid)
            {
                foreach(var cartitem in ProductImage)
                {
                    if(cartitem.Length > 0)
                    {
                        using(var stream = new MemoryStream())
                        {
                            await cartitem.CopyToAsync(stream);
                            cart.ProductImage = stream.ToArray();
                        }
                    }
                }
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Products/Cart/Delete/
        public async Task<IActionResult> DeleteCart(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(n => n.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Products/Cart/Delete/
        [HttpPost, ActionName("DeleteCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(f => f.CartID == id);
        }
    }
}
