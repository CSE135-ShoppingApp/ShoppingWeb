using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Shoppa.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string SKU { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AvailableDate { get; set; }
        
        public Category Category { get; set; }

        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }
    }

    public class ShoppaDBContext : IdentityDbContext<ApplicationUser>
    {
        public ShoppaDBContext()
            : base("ShoppaDBContext", throwIfV1Schema: false)
        {
        }

        public static ShoppaDBContext Create()
        {
            return new ShoppaDBContext();
        }
    
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<State> States { get; set; }

    }
}