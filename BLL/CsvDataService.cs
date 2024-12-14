using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL;
using DAL.Models;

namespace BLL
{
    public class CsvDataService<T> : IDataService<T> where T : class
    {
        private readonly string _csvFilePathShops;
        private readonly string _csvFilePathProducts;

        public CsvDataService(string csvFilePathShops, string csvFilePathProducts)
        {
            _csvFilePathShops = csvFilePathShops;
            _csvFilePathProducts = csvFilePathProducts;
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

        public void AddToDatabase(T entity, ApplicationDbContext? context)
        {
            var filePathShops = Path.Combine(_csvFilePathShops, "Shops.csv");
            var filePathProducts = Path.Combine(_csvFilePathShops, "Products.csv");

            {
                if (entity.GetType() == typeof(Shop))
                {
                    var writer = new StreamWriter(filePathShops, true);
                    Shop shop = entity as Shop;
                    writer.WriteLine($"{shop.Name},{shop.Address}");
                }
                else if (entity.GetType() == typeof(Product))
                {
                    var writer = new StreamWriter(filePathProducts, true);
                    Product product = entity as Product;
                    writer.WriteLine($"{product.Name},{product.ShopId},{product.Amount},{product.Price}");
                }
            }
        }

        public string GetTheFinalPrice(int productId, int amount)
        {
            var products = LoadProductsFromCsv();
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                return (product.Price * amount).ToString();
            }
            return "Продукт не найден.";
        }

        public string BuyProducts(int productId, int amount)
        {
            var products = LoadProductsFromCsv();
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                if (product.Amount >= amount)
                {
                    product.Amount -= amount;
                    SaveProductsToCsv(products);
                    return "Покупка завершена";
                }
                else
                {
                    return $"В магазине недостаточно товара. В наличии {product.Amount}.";
                }
            }
            return "Продукт не найден.";
        }

        public string FindTheCheapestShop(string productName)
        {
            var products = LoadProductsFromCsv();
            var product = products
                .Where(p => p.Name.Contains(productName))
                .OrderBy(p => p.Price)
                .FirstOrDefault();

            if (product != null)
            {
                var shops = LoadShopsFromCsv();
                var shop = shops.FirstOrDefault(s => s.Id == product.ShopId);
                return shop != null ? $"Продукт: '{product.Name}' \nМагазин: '{shop.Name}' по адресу {shop.Address}" : "Магазин не найден.";
            }

            return "Продукт не найден.";
        }

        public List<string> FindProductsToBuy(int shopId, double moneyAmount)
        {
            var products = LoadProductsFromCsv();

            return products
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
            var products = LoadProductsFromCsv();
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                product.Amount += amount;
                if (price != 0.0) product.Price = price;
                SaveProductsToCsv(products);
            }
        }

        private List<Product> LoadProductsFromCsv()
        {
            var filePath = Path.Combine(_csvFilePathProducts, "products.csv");
            var products = new List<Product>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    var product = new Product
                    {
                        Name = parts[0],
                        ShopId = int.Parse(parts[1]),
                        Amount = int.Parse(parts[2]),
                        Price = double.Parse(parts[3])
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        private void SaveProductsToCsv(List<Product> products)
        {
            var filePath = Path.Combine(_csvFilePathProducts, "products.csv");
            var lines = products.Select(p => $"{p.Name},{p.ShopId},{p.Amount},{p.Price}");
            File.WriteAllLines(filePath, lines);
        }

        private List<Shop> LoadShopsFromCsv()
        {
            var filePath = Path.Combine(_csvFilePathShops, "shops.csv");
            var shops = new List<Shop>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    var shop = new Shop
                    {
                        Name = parts[0],
                        Address = parts[1]
                    };
                    shops.Add(shop);
                }
            }
            return shops;
        }
    }
}
