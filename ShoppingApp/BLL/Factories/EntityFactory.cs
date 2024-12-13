using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL
{
    public abstract class EntityFactory<T> where T : class
    {
        public abstract T CreateEntity(params object[] parameters);

        // Метод для добавления объекта в базу данных
        public void AddToDatabase(T entity, ApplicationDbContext context)
        {
            if (typeof(T) == typeof(Shop))
            {
                context.Shops.Add(entity as Shop);
            }
            else if (typeof(T) == typeof(Product))
            {
                context.Products.Add(entity as Product);
            }

            context.SaveChanges();
        }
    }

}
