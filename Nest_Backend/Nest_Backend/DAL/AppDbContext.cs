using Microsoft.EntityFrameworkCore;
using Nest_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Categories> Categories { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImg> ProductImgs { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
