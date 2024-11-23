using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL
{
    public class ProductsPurchaser
    {
        private ApplicationDbContext applicationContext = new ApplicationDbContext();
        public ProductsPurchaser() { }

        public string GetTheFinalPrice(int productId, int amount)
        {

            return applicationContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Price * amount)
                .First()
                .ToString();
        }

        public string BuyProducts (int productId, int amount)
        {
            Product product = applicationContext.Products
                .Where(p => p.Id == productId)
                .First();
            if (product.Amount > amount) {
                product.Amount = product.Amount - amount;
                applicationContext.SaveChanges();
                return "Покупка завершена";
            } 
            else if (product.Amount == amount) {
                applicationContext.Products.Remove(product);
                applicationContext.SaveChanges();
                return "Покупка завершена. Вы купили последний товар в наличии";
            } 
            else
            {
                return $"В магазине надостаточно товара для совершения покупки. Сейчас можно купить {product.Amount}";
            }
        }
    }
}
