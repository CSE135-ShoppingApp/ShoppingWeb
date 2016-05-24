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
    public class CategoriesController : Controller
    {
        private ShoppaDBContext db = new ShoppaDBContext();

        // GET: Categories
        public ActionResult Index(string message)
        {
            ViewBag.UpdateMessage = message;
            var categories = db.Categories.Include("Products").ToList();

            return View(categories);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description")] Category category)
        {

            if (ModelState.IsValid)
            {

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    string error = "Could not add, check that the category does not exist already.";


                    try
                    {
                        var cat = db.Categories.Where(c => c.Name == category.Name).FirstOrDefault();

                        if (cat == null)
                        {
                            db.Categories.Add(category);
                            db.SaveChanges();
                            dbContextTransaction.Commit();

                            return RedirectToAction("Index", new { message ="You have created a category!" });
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

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    string error = "Could not edit, check that the category does not exist already.";


                    try
                    {
                        var cat = db.Categories.Where(c => c.Name == category.Name && c.ID != category.ID).FirstOrDefault();

                        if (cat == null)
                        {
                            db.Entry(category).State = EntityState.Modified;
                            db.SaveChanges();
                            dbContextTransaction.Commit();


                            return RedirectToAction("Index", new { message = "You have edited a category!" });

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
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = null;

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                string error = "Could not remove, check that the category is not assigned to one or more products.";


                try
                {
                    category = db.Categories.Find(id);

                    if (category == null)
                    {
                        return RedirectToAction("Index");

                    }
                    else if (category.Products.Count == 0)
                    {

                        db.Categories.Remove(category);
                        db.SaveChanges();
                        dbContextTransaction.Commit();

                        return RedirectToAction("Index", new { message = "You have deleted a category!" });
                    }

                    ModelState.AddModelError("", error);
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", error);
                }
            }

            return View(category);
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
