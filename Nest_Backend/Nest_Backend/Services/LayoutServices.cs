using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Services
{
    public class LayoutServices
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public LayoutServices(AppDbContext context , IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(p => p.Key, p => p.Value);
        }
        public int GetBasketItemCount()
        {
            if (_accessor.HttpContext.Request.Cookies["Basket"] == null)
            {
                return 0;
            }
            ICollection<BasketVM> basket = JsonConvert.DeserializeObject<ICollection<BasketVM>>(_accessor.HttpContext.Request.Cookies["Basket"]);
            return basket.Sum(b => b.Count);
        }

        public List<BasketItemsVM> GetBasket()
        {
            List<BasketVM> basketItems = new List<BasketVM>();
            List<BasketItemsVM> basketItemVMs = new List<BasketItemsVM>();

            if (_accessor.HttpContext.Request.Cookies["Basket"] != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["Basket"]);
            }

            foreach (var item in basketItems)
            {
                Product dbProduct = _context.Products.Include(p => p.ProductImgs).FirstOrDefault(P => P.Id == item.ProductId);
                if (dbProduct == null) continue;
                BasketItemsVM basketItem = new BasketItemsVM
                {
                    ProductId = dbProduct.Id,
                    Image = dbProduct.ProductImgs.FirstOrDefault(pi => pi.IsFront).Image,
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Raiting = dbProduct.Raiting,
                    IsActive = dbProduct.StockCount > 0 ? true : false,
                    Count = item.Count
                };
                basketItemVMs.Add(basketItem);
            }
            return basketItemVMs;
        }


    }
}
