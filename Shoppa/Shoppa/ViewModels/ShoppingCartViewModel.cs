using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shoppa.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int ID { get; set; }
        public List<Cart> CartItems { get; set; }

        [Display(Name="Cart Total")]
        [DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}