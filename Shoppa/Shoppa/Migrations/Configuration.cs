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
            AddUserAndRole(context);


            context.States.AddOrUpdate(i => i.Name,
                new State
                {
                    Name = "AL",
                    FullName = "Alabama"
                },
                new State
                {
                    Name = "CA",
                    FullName = "California"
                },
                new State
                {
                    Name = "CO",
                    FullName = "Colorado"
                },
                new State
                {
                    Name = "CT",
                    FullName = "Connecticut"
                },
                new State
                {
                    Name = "DE",
                    FullName = "Delaware"
                },
                new State
                {
                    Name = "FL",
                    FullName = "Florida"
                },
                new State
                {
                    Name = "GA",
                    FullName = "Georgia"
                },
                new State
                {
                    Name = "HI",
                    FullName = "Hawaii"
                },
                new State
                {
                    Name = "ID",
                    FullName = "Idaho"
                },
                new State
                {
                    Name = "IL",
                    FullName = "Illinois"
                },
                new State
                {
                    Name = "IN",
                    FullName = "Indiana"
                },
                new State
                {
                    Name = "IA",
                    FullName = "Iowa"
                },
                new State
                {
                    Name = "KS",
                    FullName = "Kansas"
                },
                new State
                {
                    Name = "KY",
                    FullName = "Kentucky"
                },
                new State
                {
                    Name = "LA",
                    FullName = "Louisiana"
                },


                new State
                {
                    Name = "MD",
                    FullName = "Maryland"
                },
                new State
                {
                    Name = "MA",
                    FullName = "Massachusetts"
                },
                new State
                {
                    Name = "MI",
                    FullName = "Michigan"
                },
                new State
                {
                    Name = "MN",
                    FullName = "Minnesota"
                },
                new State
                {
                    Name = "MS",
                    FullName = "Mississippi"
                },
                new State
                {
                    Name = "ME",
                    FullName = "Maine"
                },
                new State
                {
                    Name = "MT",
                    FullName = "Montana"
                },
                new State
                {
                    Name = "NE",
                    FullName = "Nebraska"
                },
                new State
                {
                    Name = "NV",
                    FullName = "Nevada"
                },
                new State
                {
                    Name = "NH",
                    FullName = "New Hampshire"
                },
                new State
                {
                    Name = "NJ",
                    FullName = "New Jersey"
                },
                new State
                {
                    Name = "NM",
                    FullName = "New Mexico"
                },
                new State
                {
                    Name = "MO",
                    FullName = "Missouri"
                },

                new State
                {
                    Name = "NY",
                    FullName = "New York"
                },
                new State
                {
                    Name = "NC",
                    FullName = "North Carolina"
                },
                        new State
                        {
                            Name = "ND",
                            FullName = "North Dakota"
                        },
                    new State
                    {
                        Name = "OH",
                        FullName = "Ohio"
                    },
                        new State
                        {
                            Name = "OK",
                            FullName = "Oklahoma"
                        },
                    new State
                    {
                        Name = "OR",
                        FullName = "Oregon"
                    },
                    new State
                    {
                        Name = "PA",
                        FullName = "Pennsylvania"
                    },
                    new State
                    {
                        Name = "RI",
                        FullName = "Rhode Island"
                    },

                new State
                {
                    Name = "AR",
                    FullName = "Arkansas"
                },
                new State
                {
                    Name = "AZ",
                    FullName = "Arizona"
                },
                new State
                {
                    Name = "AK",
                    FullName = "Alaska"
                },




            new State
            {
                Name = "SC",
                FullName = "South Carolina"
            },
            new State
            {
                Name = "SD",
                FullName = "South Dakota"
            },
            new State
            {
                Name = "TN",
                FullName = "Tennessee"
            },
            new State
            {
                Name = "TX",
                FullName = "Texas"
            },
            new State
            {
                Name = "UT",
                FullName = "Utah"
            },
            new State
            {
                Name = "VT",
                FullName = "Vermont"
            },
            new State
            {
                Name = "VA",
                FullName = "Virginia"
            },
            new State
            {
                Name = "WA",
                FullName = "Washington"
            },
            new State
            {
                Name = "WV",
                FullName = "West Virginia"
            },
            new State
            {
                Name = "WI",
                FullName = "Wisconsin"
            },
            new State
            {
                Name = "WY",
                FullName = "Wyoming"
            }
                );

            context.Categories.AddOrUpdate(i => i.Name,
                new Category
                {
                    Name = "TVs"
                },
                new Category
                {
                    Name = "Laptops"
                },
                new Category
                {
                    Name = "Tablets"
                }
            );

            context.Products.AddOrUpdate(i => i.Name,

                new Product
                {
                    SKU = "TSHIBA50LTV",
                    Rating = 4.5M,
                    Name = "Toshiba 50\" LED TV",
                    AvailableDate = DateTime.Parse("2015-1-11"),
                    Category = context.Categories.Where(c => c.Name == "TVs").FirstOrDefault(),
                    Price = 300.99M
                },

                new Product
                {
                    SKU = "MCBOOKP15R",
                    Name = "Macbook Pro 15.6\" Retina",
                    Rating = 4.9M,
                    AvailableDate = DateTime.Parse("2014-3-13"),
                    Category = context.Categories.Where(c => c.Name == "Laptops").FirstOrDefault(),
                    Price = 800.99M
                },

                new Product
                {
                    Name = "The New Razer Blade",
                    SKU = "RZRBLD2016",
                    Rating = 4.7M,
                    AvailableDate = DateTime.Parse("2016-2-23"),
                    Category = context.Categories.Where(c => c.Name == "Laptops").FirstOrDefault(),
                    Price = 999.99M
                },

                new Product
                {
                    Name = "iPad Pro 9.7\"",
                    SKU = "IPAD97",
                    Rating = 4.3M,
                    AvailableDate = DateTime.Parse("1959-4-15"),
                    Category = context.Categories.Where(c => c.Name == "Tablets").FirstOrDefault(),
                    Price = 500.99M,
                }
           );
        }

        bool AddUserAndRole(Shoppa.Models.ShoppaDBContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("isOwner"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "user1@contoso.com",
            };
            ir = um.Create(user, "P_assw0rd1");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "isOwner");
            return ir.Succeeded;
        }
    }
}
