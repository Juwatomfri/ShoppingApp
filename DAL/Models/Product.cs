﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product : Entity
    {
        public int ShopId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
