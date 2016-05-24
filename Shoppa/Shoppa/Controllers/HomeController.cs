using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    public class HomeController : Controller
    {
        ShoppaDBContext context = new ShoppaDBContext();

        public ActionResult Index(string message)
        {
            ViewBag.UpdateMessage = message;

            // Get most popular products
            var products = GetTopSellingProducts(5);

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Simple shopping cart application with product & category management.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<Product> GetTopSellingProducts(int count)
        {
            // Group the order details by product and return
            // the procut with the highest count
            return context.Products
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}