using Shoppa.Models;
using Shoppa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoppa.Controllers
{
    public class ShoppingCartController : Controller
    {
        ShoppaDBContext storeDB = new ShoppaDBContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index(string message)
        {
            ViewBag.UpdateMessage = message;

            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id, int qty)
        {

            // Retrieve the product from the database
            var addedProduct = storeDB.Products
                .Single(product => product.ID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct, qty);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index", new { message = qty.ToString() + " " + addedProduct.Name + " have been added to your cart"});
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the product to display confirmation
            string productName = storeDB.Carts
                .Single(item => item.ID == id).Product.Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteID = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}