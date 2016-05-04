using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Shoppa.Models
{
    public class ShoppingCartItem
    {
        public int CartID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }
    }
}