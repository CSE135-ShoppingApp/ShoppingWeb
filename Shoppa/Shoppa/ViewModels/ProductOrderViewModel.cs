using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shoppa.ViewModels
{
    public class ProductOrderViewModel
    {
        public int ID { get; set; }

        public Product Product { get; set; }

        public int ProductID { get; set; }

        [Required]
        [Range(1, 99)]
        public int Quantity { get; set; }
    }
}