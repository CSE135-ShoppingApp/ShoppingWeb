using Shoppa.Models;
using Shoppa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    public class StoreController : Controller
    {
        ShoppaDBContext context = new ShoppaDBContext();

        //
        // GET: /Store/
        public ActionResult Index(string searchString, string productCategory)
        {
            // Get categories list
            var CategoriesList = context.Categories.Select(p => p.Name).Distinct().ToList();

            ViewBag.ProductCategory = new SelectList(CategoriesList, productCategory);

            var products = context.Products.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
                ViewBag.SearchString = searchString;
            }

            if (!string.IsNullOrEmpty(productCategory))
            {
                products = products.Where(x => x.Category != null && x.Category.Name == productCategory);
            }

            return View(products.ToList());
        }

        //
        // GET: /Store/Browse?category=TVs
        public ActionResult Browse(string category)
        {
            var categories = context.Categories.Include("Products")
                .Single(g => g.Name == category);

            return View(categories);
        }

        //
        // GET: /Store/CategoryMenu

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = context.Categories.ToList();
            return PartialView(categories);
        }

        //
        // GET: /Store/Details/5
        public ActionResult Details(int id)
        {
            var product = context.Products.Find(id);

            var vm = new ProductOrderViewModel() { Product = product, ProductID = product.ID, Quantity = 1 };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Details([Bind(Include = "ID,ProductID,Quantity")] ProductOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("AddToCart", "ShoppingCart", new { id = model.ProductID, qty = model.Quantity });
            }

            model.Product = context.Products.Find(model.ProductID);

            return View(model);
        }

    }
}