using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        ShoppaDBContext context = new ShoppaDBContext();

        //
        // GET: /Store/
        public ActionResult Index()
        {
            return View(context.Categories.ToList());
        }

        //
        // GET: /Store/Browse?category=TVs
        public ActionResult Browse(string category)
        {
            var cat = new Category { Name = category };
            return View(cat);
        }

        //
        // GET: /Store/Details/5
        public ActionResult Details(int id)
        {
            var product = new Product { Name = "Product " + id };
            return View(product);
        }
    }
}