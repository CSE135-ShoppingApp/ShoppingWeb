using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    [AllowAnonymous]
    public class AnalyticsController : Controller
    {
        shoppa135dbEntities context = new shoppa135dbEntities();

        // GET: Analytics
        public ActionResult Sales(string rowsMenu, string orderMenu, string productCategories)
        {
            // FILTERING OPTIONS:
            string[] rowsMenuList = new string[] { "Customers", "States" };
            // Select default and assign list
            ViewBag.RowsMenu = new SelectList(rowsMenuList, rowsMenu ?? "Customers");
            string[] orderMenuList = new string[] { "Alphabetical", "Top-K" };
            ViewBag.OrderMenu = new SelectList(orderMenuList, orderMenu ?? "Alphabetical");
            var productCategoriesList = context.categories.Select(x => x.name).ToList();
            ViewBag.ProductCategories = new SelectList(productCategoriesList, productCategories ?? "All");

            // REPORT BUILDER:
            cont

            return View();
        }
    }
}