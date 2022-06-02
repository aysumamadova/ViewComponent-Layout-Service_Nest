using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.ViewModels
{
    public class BasketItemsVM
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public float Raiting { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int Count { get; set; }
    }
}
