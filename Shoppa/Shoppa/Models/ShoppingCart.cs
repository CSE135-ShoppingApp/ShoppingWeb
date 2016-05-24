using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
namespace Shoppa.Models
{
    public partial class ShoppingCart
    {
        ShoppaDBContext storeDB = new ShoppaDBContext();
        string ShoppingCartID { get; set; }
        public const string CartSessionKey = "CartID";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartID(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Product product, int qty)
        {

            // Get the matching cart and product instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartID == ShoppingCartID
                && c.ProductID == product.ID);
 
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductID = product.ID,
                    Price = product.Price,
                    CartID = ShoppingCartID,
                    Count = qty,
                    CreatedDate = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count += qty;
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.Carts.Single(
                cart => cart.CartID == ShoppingCartID 
                && cart.ID == id);
 
            int itemCount = 0;
 
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                // Save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(
                cart => cart.CartID == ShoppingCartID);
 
            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(
                cart => cart.CartID == ShoppingCartID).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartID == ShoppingCartID
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply product price by count of that prodcut to get 
            // the current price for each of those product in the cart
            // sum all product price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartID == ShoppingCartID
                              select (int?)cartItems.Count *
                              cartItems.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
 
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductID = item.ProductID,
                    OrderID = order.ID,
                    UnitPrice = item.Price,
                    Quantity = item.Count,
                    Order = order,
                    Product = item.Product
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Price);
                storeDB.SaveChanges();

                storeDB.OrderDetails.Add(orderDetail);
                storeDB.SaveChanges();

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            storeDB.Entry(order).State = EntityState.Modified;

            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.ID;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartID(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartID = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartID.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(
                c => c.CartID == ShoppingCartID);
 
            foreach (Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            storeDB.SaveChanges();
        }
    }
}