﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Models
{
    public class Shop
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
