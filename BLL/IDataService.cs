using DAL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IDataService<T> 
    {
        Entity CreateEntity(string entType, params object[] parameters);

        public void AddToDatabase(T entity, ApplicationDbContext? context);

        string GetTheFinalPrice(int productId, int amount);

        string BuyProducts(int productId, int amount);

        string FindTheCheapestShop(string productName);

        List<string> FindProductsToBuy(int shopId, double moneyAmount);

        void SupplyProduct(int productId, int amount, double price);
    }
}
