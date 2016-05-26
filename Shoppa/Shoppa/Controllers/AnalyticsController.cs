using Shoppa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public ActionResult SimilarProducts()
        {
            Data d = new Data();
            d.PerformQuery2();
            return View(d);
        }

        // GET: Analytics
        public ActionResult Sales()
        {
            // FILTERING OPTIONS:
            var rowsMenuList =
                new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string,string>("States", "state"), 
                      new KeyValuePair<string,string>("Customers", "name")};

            var orderMenuList =
                new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string,string>("Alphabetical", "alphabet"), 
                      new KeyValuePair<string,string>("Top-K", "topk")};

            // Get Categories
            var categories = context.categories.ToList();

            int currentPage = 0;
            int currentColumn = 0;
            string rowsMenu = "name";
            string orderMenu = "alphabet";
            int productCategories = 0;

            string rowsCriteria = "Customers";

            int startRow = 0;
            int startColumn = 0;
            bool hideNextCol = false;
            bool hideNextPage = false;

            int maxCols = context.products.Count();

            if (startColumn + 9 >= maxCols)
            {
                hideNextCol = true;
            }

            int maxRows = context.users.Count();
     
            if (startRow + 19 >= maxRows)
            {
                hideNextPage = true;
            }

            // Select default and assign list
            ViewBag.RowsMenu = new SelectList(rowsMenuList, "Value", "Key", rowsMenu);
            ViewBag.OrderMenu = new SelectList(orderMenuList, "Value", "Key", orderMenu);
            ViewBag.CurrentPage = currentPage;
            ViewBag.CurrentColumn = currentColumn;
            ViewBag.ProductCategories = new SelectList(categories, "id", "name", productCategories);
            ViewBag.RowsCriteria = rowsCriteria;
            ViewBag.ShowNextPage = !hideNextPage;
            ViewBag.ShowNextCol = !hideNextCol;
     
            Data d = new Data();
            d.PerformQuery(rowsMenu, orderMenu, startRow, startColumn, productCategories);
            return View(d);
        }

        [HttpPost]
        public ActionResult Sales(FormCollection fc, string rowsMenu = "name", string orderMenu = "alphabet", int productCategories = 0, string button = "Run")
        {
            // FILTERING OPTIONS:
            var rowsMenuList =
                new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string,string>("States", "state"), 
                      new KeyValuePair<string,string>("Customers", "name")};

            var orderMenuList =
                new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string,string>("Alphabetical", "alphabet"), 
                      new KeyValuePair<string,string>("Top-K", "topk")};

            // Get Categories
            var categories = context.categories.ToList();

            string rowsCriteria = rowsMenu == "name" ? "Customers" : "States";

            
            int currentPage = int.Parse(fc["currentPage"] ?? "0");
            int currentColumn = int.Parse(fc["currentColumn"] ?? "0");

            if (button == "Next 20 " + rowsCriteria)
            {
                currentPage++;
            }
            else if (button == "Next 10 Products")
            {
                currentColumn++;
            }

            int startRow = (currentPage * 20) + 1;
            int startColumn = (currentColumn * 10) + 1;

            bool readOnly = false;
            bool hideNextPage = false;
            bool hideNextCol = false;

            if (currentColumn != 0) 
            {
                readOnly = true;
            }

            int maxCols = 0;
            if (productCategories != 0)
            {
                maxCols = context.products.Count(x => x.category_id == productCategories);
            }
            else
            {
                maxCols = context.products.Count();
            }

            if (startColumn + 9 >= maxCols)
            {
                hideNextCol = true;
            }
            
            if (currentPage != 0) 
            {
                readOnly = true;
            }

            int maxRows = 0;
            if (rowsCriteria == "Customers")
            {
                maxRows = context.users.Count();
            }
            else
            {
                maxRows = context.users.Select(x => x.state).Distinct().Count();
            }

            if (startRow + 19 >= maxRows)
            {
                hideNextPage = true;
            }

            // Select default and assign list
            ViewBag.RowsMenu = new SelectList(rowsMenuList, "Value", "Key", rowsMenu);
            ViewBag.OrderMenu = new SelectList(orderMenuList, "Value", "Key", orderMenu);
            ViewBag.CurrentPage = currentPage;
            ViewBag.CurrentColumn = currentColumn;
            ViewBag.ProductCategories = new SelectList(categories, "id", "name", productCategories);
            ViewBag.RowsCriteria = rowsCriteria;
            ViewBag.ReadOnly = readOnly;
            ViewBag.ShowNextPage = !hideNextPage;
            ViewBag.ShowNextCol = !hideNextCol;

            Data d = new Data();
            d.PerformQuery(rowsMenu, orderMenu, startRow, startColumn, productCategories);
            return View(d);
        }
    }
}