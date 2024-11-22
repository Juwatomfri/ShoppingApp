using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL.Factories
{
    public class ProductFactory : EntityFactory<Product>
    {
        /// <summary>
        /// Функция добавления нового продукта
        /// </summary>
        /// <param name="parameters">Тут: name, shopId, amount и price</param>
        /// <returns></returns>
        public override Product CreateEntity(params object[] parameters)
        {
            return new Product
            {
                Name = parameters[0] as string,
                ShopId = (int)parameters[1],
                Amount = (int)parameters[2],
                Price = (double)parameters[3]
            };
        }
    }
}
