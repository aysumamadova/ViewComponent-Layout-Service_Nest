using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bu xana boş ola bilməz"),MaxLength(150)]
        public string Name { get; set; }
        public string Info { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public double Price { get; set; }
        public float Raiting { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public int CategoriesId { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public int StockCount { get; set; }
        public bool IsDeleted { get; set; } 
        public Categories Categories { get; set; }
        public List<ProductImg>ProductImgs { get; set; }
    }
}
