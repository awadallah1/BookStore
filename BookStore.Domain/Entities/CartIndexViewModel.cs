﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
   public  class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string Returnurl { get; set; }
    }
}
