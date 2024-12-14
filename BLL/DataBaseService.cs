using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;

namespace BLL
{
    public class DatabaseDataService<T> : IDataService<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public DatabaseDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Entity CreateEntity(string entType, params object[] parameters)
        {
            if (entType == "product")
            {

                return new Product
                {
                    Name = parameters[0] as string,
                    ShopId = (int)parameters[1],
                    Amount = (int)parameters[2],
                    Price = (double)parameters[3]
                };
            }
            else
            {
                return new Shop
                {
                    Name = parameters[0] as string,
                    Address = parameters[1] as string,
                };
            }
        }

        public void AddToDatabase(T entity, ApplicationDbContext context)
        {
            if (entity.GetType() == typeof(Shop))
            {
                context.Shops.Add(entity as Shop);
            }
            else if (entity.GetType() == typeof(Product))
            {
                context.Products.Add(entity as Product);
            }

            context.SaveChanges();
        }

        public string GetTheFinalPrice(int productId, int amount)
        {
            return _context.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Price * amount)
                .First()
                .ToString();
        }

        public string BuyProducts(int productId, int amount)
        {
            Product product = _context.Products
                .Where(p => p.Id == productId)
                .First();
            if (product.Amount > amount)
            {
                product.Amount = product.Amount - amount;
                _context.SaveChanges();
                return "Покупка завершена";
            }
            else if (product.Amount == amount)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return "Покупка завершена. Вы купили последний товар в наличии";
            }
            else
            {
                return $"В магазине надостаточно товара для совершения покупки. Сейчас можно купить {product.Amount}";
            }
        }

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

        public List<string> FindProductsToBuy(int shopId, double moneyAmount)
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

        public void SupplyProduct(int productId, int amount, double price)
        {
            Product product = _context.Products.First(p => p.Id == productId);
            if (product != null)
            {
                product.Amount += amount;
                if (price != 0.0) product.Price = price;
                _context.SaveChanges();
            }
        }
    }
}
