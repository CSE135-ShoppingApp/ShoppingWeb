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
        public ActionResult Index(string message)
        {
            ViewBag.UpdateMessage = message;

            return View();
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
    }
}