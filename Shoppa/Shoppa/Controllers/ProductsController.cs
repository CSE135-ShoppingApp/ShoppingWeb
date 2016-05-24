using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shoppa.Models;

namespace Shoppa.Controllers
{
    [AuthorizeUserAttribute(Roles = "isOwner")]
    public class ProductsController : Controller
    {
        private ShoppaDBContext db = new ShoppaDBContext();

        // GET: Products
        public ActionResult Index(string productCategory, string searchString, string message, bool? isGetBack)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                if (isGetBack.HasValue && isGetBack.Value)
                {
                    TempData["selectedSearchString"] = null;
                }
                else
                {
                    searchString = (string)TempData["selectedSearchString"];
                }
            }
             
            if (string.IsNullOrEmpty(productCategory))
            {
                if (isGetBack.HasValue && isGetBack.Value)
                {
                    TempData["selectedProductCategory"] = null;
                }
                else
                {
                    productCategory = (string)TempData["selectedProductCategory"];
                }
            }

            ViewBag.UpdateMessage = message;

            // Get categories list
            var CategoriesList = db.Categories.Select(p => p.Name).Distinct().ToList();

            ViewBag.ProductCategory = new SelectList(CategoriesList, productCategory);

            var products = db.Products.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
                ViewBag.SearchString = searchString;
                TempData["selectedSearchString"] = searchString;
            }

            if (!string.IsNullOrEmpty(productCategory))
            {
                products = products.Where(x => x.Category != null && x.Category.Name == productCategory);
                TempData["selectedProductCategory"] = productCategory;
            }

            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID",
"Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SKU,Name,AvailableDate,CategoryID,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    string error = "Could not add, check that a product with the same SKU does not exist already.";

                    try
                    {
                        var p = db.Products.Where(c => c.SKU == product.SKU).FirstOrDefault();

                        if (p == null)
                        {
                            db.Products.Add(product);
                            db.SaveChanges();
                            dbContextTransaction.Commit();

                            return RedirectToAction("Index", new { message = "You have added a product!" });
                        }


                        ModelState.AddModelError("", error);
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        ModelState.AddModelError("", error);
                    }
                }

           
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID",
"Name", product.CategoryID);

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();

                
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID",
   "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SKU,Name,AvailableDate,CategoryID,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    string error = "Could not add, check that a product with the same SKU does not exist already.";

                    try
                    {
                        var p = db.Products.Where(c => c.SKU == product.SKU && c.ID != product.ID).FirstOrDefault();

                        if (p == null)
                        {

                            db.Entry(product).State = EntityState.Modified;
                            db.SaveChanges();
                            dbContextTransaction.Commit();

                            return RedirectToAction("Index", new { message = "You have edited a product!" });
                        }

                        ModelState.AddModelError("", error);
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        ModelState.AddModelError("", error);
                    }
                }

            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID",
   "Name", product.CategoryID);

            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {



            Product product = null;

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                string error = "Could not remove, check that the product has not been bought yet.";

                try
                {
                    product = db.Products.Find(id);

                    if (product == null)
                    {
                        return RedirectToAction("Index");

                    }
                    else if (product.OrderDetails.Count == 0)
                    {

                        db.Products.Remove(product);
                        db.SaveChanges();
                        dbContextTransaction.Commit();

                        return RedirectToAction("Index", new { message = "You have deleted a product!" });
                    }

                    ModelState.AddModelError("", error);
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", error);
                }
            }

            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
