namespace Shoppa.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shoppa.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shoppa.Models.ShoppaDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
           
        }

        protected override void Seed(Shoppa.Models.ShoppaDBContext context)
        {
            ApplicationUser user = AddUserAndRole(context);

            context.Categories.AddOrUpdate(i => i.Name , 
            
            new Category {
                Name = "TVs",
                Description = "Televisions, ranging from LCD and Plasma to LED"
            },
            new Category
            {
                Name = "Tablets", Description = "Tablets including Android and Apple"
            },
            new Category
            {
                Name = "Laptops", Description = "Laptops ranging from ultaportable to full-featured gaming machines"
            }
            );

            context.SaveChanges();

            context.Products.AddOrUpdate(i => i.Name,

                new Product
                {
                    SKU = "TSHIBA50LTV",
                    Rating = 4.5M,
                    Name = "Toshiba 50\" LED TV",
                    AvailableDate = DateTime.Parse("2015-1-11"),
                    CategoryID = context.Categories.Where(c => c.Name == "TVs").FirstOrDefault().ID,
                    Price = 300.99M
                },

                new Product
                {
                    SKU = "MCBOOKP15R",
                    Name = "Macbook Pro 15.6\" Retina",
                    Rating = 4.9M,
                    AvailableDate = DateTime.Parse("2014-3-13"),
                    CategoryID = context.Categories.Where(c => c.Name == "Laptops").FirstOrDefault().ID,
                    Price = 800.99M
                },

                new Product
                {
                    Name = "The New Razer Blade",
                    SKU = "RZRBLD2016",
                    Rating = 4.7M,
                    AvailableDate = DateTime.Parse("2016-2-23"),
                    CategoryID = context.Categories.Where(c => c.Name == "Laptops").FirstOrDefault().ID,
                    Price = 999.99M
                },

                new Product
                {
                    Name = "iPad Pro 9.7\"",
                    SKU = "IPAD97",
                    Rating = 4.3M,
                    AvailableDate = DateTime.Parse("1959-4-15"),
                    CategoryID = context.Categories.Where(c => c.Name == "Tablets").FirstOrDefault().ID,
                    Price = 500.99M,
                }
           );
        }

        ApplicationUser AddUserAndRole(Shoppa.Models.ShoppaDBContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("isOwner"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "jose", State = State.CA, Age = 25,  Role = Roles.Owner
            };
            ir = um.Create(user, ApplicationUser.GenericPassword);
            if (ir.Succeeded == false)
                return null;
            ir = um.AddToRole(user.Id, "isOwner");
            return user;
        }
    }
}
