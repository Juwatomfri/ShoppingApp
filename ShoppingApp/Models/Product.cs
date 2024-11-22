using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }


        public Product(int id, string name, int shopId, int amount, double price)
        {
            Id = id;
            Name = name;
            ShopId = shopId;
            Amount = amount;
            Price = price;
        }
    }
}
