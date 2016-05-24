using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Shoppa.Models
{

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Shoppa.ViewModels.ShoppingCartViewModel> ShoppingCartViewModels { get; set; }
    }
}