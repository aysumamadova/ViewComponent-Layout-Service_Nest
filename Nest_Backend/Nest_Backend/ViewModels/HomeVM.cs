using Nest_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.ViewModels
{
    public class HomeVM
    {
        public List<Slider>Sliders { get; set; }
        public List<Categories>Categories { get; set; }
        public List<Product>Recently { get; set; }
        public List<Product> TopRated { get; set; }
    }
}
