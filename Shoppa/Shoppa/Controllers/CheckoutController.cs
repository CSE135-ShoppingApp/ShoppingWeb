using Shoppa.Models;
using Shoppa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    public class CheckoutController : Controller
    {

        ShoppaDBContext storeDB = new ShoppaDBContext();
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            ViewBag.Cart = viewModel;

            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var shoppCart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = shoppCart.GetCartItems(),
                CartTotal = shoppCart.GetTotal()
            };

            var order = new Order();

            using (var dbContextTransaction = storeDB.Database.BeginTransaction())
            {

                try
                {
                    TryUpdateModel(order);

                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    shoppCart.CreateOrder(order);
                    dbContextTransaction.Commit();

                    return RedirectToAction("Complete",
                        new { id = order.ID });
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", "There was an error while processing your order, please try again.");
                }
            }

            ViewBag.Cart = viewModel;
            return View(order);
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.ID == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}