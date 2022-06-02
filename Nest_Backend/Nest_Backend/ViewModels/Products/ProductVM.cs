using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.ViewModels.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; }

    }
}
