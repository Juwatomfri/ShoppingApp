using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL
{
    public class PossibleProductsFinder
    {
        public PossibleProductsFinder() { }

        public List<string> Find(int shopId, double moneyAmount)
        {
            ApplicationDbContext applicationContext = new ApplicationDbContext();
           
            return applicationContext.Products
            .Where(p => p.ShopId == shopId && p.Price > 0)
            .Select(p => new
            {
                p.Name,
                MaxQuantity = (int)(moneyAmount / p.Price) > p.Amount ? p.Amount : (int)(moneyAmount / p.Price),
            })
            .Where(p => p.MaxQuantity > 0)
            .Select(p => $"{p.Name}: {p.MaxQuantity} шт.")
            .ToList();
        }
    }
}
