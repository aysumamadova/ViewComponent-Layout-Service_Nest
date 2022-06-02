using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Areas.Manage.Controllers
{
        [Area("Manage")]
    public class ProductController : Controller
    {
        private   AppDbContext _context { get; }
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>  Index()
        {
            List<Product> products = await _context.Products.Include(p => p.ProductImgs).Include(p => p.Categories).ToListAsync();
            List<ProductVM> productVMs = new List<ProductVM>();
            foreach (var item in products)
            {
               ProductVM productVM = new ProductVM
               {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Categories.Name,
                    Price = item.Price,
                    Img = item.ProductImgs.FirstOrDefault(pi => pi.IsFront == true).Image,
                    IsDeleted=item.IsDeleted

                };
                productVMs.Add(productVM);
            }
            return View(productVMs);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
             ViewBag.Categories = _context.Categories.Where(c=>c.IsDeleted==false).ToList();
             return View();
            }
            if (_context.Products.Any(p=>p.Name.Trim().ToLower()== product.Name.Trim().ToLower()))
            {
                ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
                ModelState.AddModelError("Name", "This name already exist");
                return View();
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null) return NotFound();
            if (product.IsDeleted == true)
            {
                _context.Products.Remove(product);
            }
            else
            {
                product.IsDeleted = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product)
        {
            Product p = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (p == null) return NotFound();

            p.Name = product.Name;
            p.Info = product.Info;
            p.Price = product.Price;
            p.Raiting = product.Raiting;
            //p.CategoriesId = product.CategoriesId;
            p.StockCount = product.StockCount;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
   

}
