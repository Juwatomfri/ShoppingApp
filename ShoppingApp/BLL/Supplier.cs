using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL
{
    public class Supplier
    {
        ApplicationDbContext applicationContext = new ApplicationDbContext();
        public void SupplyProduct(int productId, int amount, double price)
        {
            Product product = applicationContext.Products.First(p => p.Id == productId);
            if (product != null)
            {
                product.Amount += amount;
                if (price != 0.0) product.Price = price;
                applicationContext.SaveChanges();
            }
        }

        public Supplier () { }
    }
}
