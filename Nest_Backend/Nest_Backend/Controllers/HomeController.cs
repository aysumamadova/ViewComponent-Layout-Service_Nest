using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductImgs).Include(p => p.Categories).AsQueryable();
            HomeVM homeVM = new HomeVM()
            {
                Sliders = await _context.Sliders.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Recently = await query.OrderByDescending(p => p.Id).Take(3).ToListAsync(),
                TopRated = await query.OrderByDescending(p => p.Raiting).Take(3).ToListAsync()

            };
            return View(homeVM);
        }
    }
}
