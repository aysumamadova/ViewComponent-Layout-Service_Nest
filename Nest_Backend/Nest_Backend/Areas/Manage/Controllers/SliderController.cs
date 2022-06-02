using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.Utilies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private AppDbContext _context { get; }

        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public ActionResult Index()
        {
            return View(_context.Sliders);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Slider slider)
        {
            if (slider.Photo.CheckSize(900))
            {
                ModelState.AddModelError("Photo", "File size must be less than 900kb");
                return View();
            }
            if (!slider.Photo.CheckType("image/"))
            {
                ModelState.AddModelError("Photo", "File must be image");
                return View();
            }
            slider.Image = await slider.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider"));
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return NotFound();
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", slider.Image)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", slider.Image));
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider slider =_context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Slider slider)
        {
            if (ModelState.IsValid)
            {
                var s = await _context.Sliders.FindAsync(slider.Id);
                s.Name = slider.Name;
                s.Info = slider.Info;
                if (slider.Photo != null)
                {
                    if (slider.Image != null)
                    {
                        string filePath = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", slider.Image);
                        System.IO.File.Delete(filePath);
                    }
                    s.Image = ProcessUploadedFile(slider);
                }
                _context.Update(s);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        private string ProcessUploadedFile(Slider slider)  
        {  
            string uniqueFileName = null;  
  
            if (slider.Photo != null)  
            {  
                string uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider");  
                uniqueFileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;  
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
                using (var fileStream = new FileStream(filePath, FileMode.Create))  
                {
                    slider.Photo.CopyTo(fileStream);  
                }  
            }  
  
            return uniqueFileName;  
        }  
    }
}
