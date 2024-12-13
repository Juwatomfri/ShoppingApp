using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL
{
    public class CheaperProductFinder
    {
        public CheaperProductFinder() { }

        public string FindTheCheapestShop(string productName)
        {
            ApplicationDbContext applicationContext = new ApplicationDbContext();
            try
            {
                Product product = applicationContext.Products
                .Where(p => (p.Name.Contains(productName)))
                .OrderBy(p => p.Price)
                .FirstOrDefault();
                if (product != null)
                {
                    Shop shop = applicationContext.Shops
                        .Where(s => s.Id == product.ShopId)
                        .First();
                    return $"Продукт: '{product.Name}' \nМагазин: \'{shop.Name}\' " + $"по адресу {shop.Address}";
                }
                else
                {
                    return "Такой товар не найден. Пожалуйста, попробуйте написать с большой буквы.";
                }

            }
            catch
            {
                throw new Exception("Ошибка поиска по базе данных");
            }
        }  
    }
}
