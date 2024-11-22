using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.BLL.Factories
{
    public class ShopFactory : EntityFactory<Shop>
    {
        public override Shop CreateEntity(params object[] parameters)
        {
            return new Shop
            {
                Name = parameters[0] as string,
                Address = parameters[1] as string,
            };
        }
    }
}
