using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Mvc;

namespace Shoppa.Models
{
    public class Product
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Index("IX_UniqueSKU", 1, IsUnique = true)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The SKU length should be between 3 and 50 characters")]
        [Required(ErrorMessage="An SKU is required")]
        public string SKU { get; set; }

        [Required(ErrorMessage="A name is required")]
        [StringLength(160, MinimumLength = 3, ErrorMessage="The name length should be between 3 and 160 characters")]
        public string Name { get; set; }

        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AvailableDate { get; set; }

        [Display(Name="Category")]
        [Required(ErrorMessage="A category must be selected")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage="Price is required")]
        [Range(0.01, 10000.00, ErrorMessage="Price must be between 0.01 and 10,000.00")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Range(1, 5)]
        public decimal Rating { get; set; }

        public virtual Category Category { get; set; }


        public virtual List<OrderDetail> OrderDetails { get; set; }

    }

}