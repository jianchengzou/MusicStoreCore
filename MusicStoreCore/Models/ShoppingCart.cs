using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MusicStoreCore.Models
{
    public partial class ShoppingCart
    {
        private MusicStoreDbContext _context;
        private string _shoppingCartId;
        public const string CartSessionKey = "CartId";
        private ShoppingCart(MusicStoreDbContext context, string shoppingCartId)
        {
            _context = context;
            _shoppingCartId = shoppingCartId;
        }

        public static ShoppingCart GetCart(MusicStoreDbContext context, HttpContext httpContext)
        {
            return new ShoppingCart(context, GetCartId(httpContext));
        }

        private static string GetCartId(HttpContext httpContext)
        {
            var cartId = httpContext.Session.GetString(CartSessionKey);

            if (cartId == null)
            {
                cartId = Guid.NewGuid().ToString();

                httpContext.Session.SetString(CartSessionKey, cartId);
            }

            return cartId;
        }

        public void AddToCart(Album album)
        {
            var cartItem = _context.CartItems.SingleOrDefault(c => c.AlbumId == album.AlbumId && c.ShoppingCartId == _shoppingCartId);

            if (cartItem == null)
            {
                //create a new cart
                var newCartItem = new CartItem() {
                    ShoppingCartId = _shoppingCartId,
                    AlbumId = album.AlbumId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _context.Add(newCartItem);
            }
            else
            {
                cartItem.Count++;
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Return CartItems count</returns>
        public int RemoveFromCart(int recordId)
        {
            var cartItem = _context.CartItems.SingleOrDefault(c => c.ShoppingCartId == _shoppingCartId && c.RecordId == recordId);

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
                    _context.CartItems.Remove(cartItem);
                }
                _context.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _context.CartItems.Where(c => c.ShoppingCartId == _shoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            var cartItems = _context.CartItems.Where(cartItem => cartItem.ShoppingCartId == this._shoppingCartId).Include(nameof(Album)).ToList();
            return cartItems;
        }

        // Get the count of each item in the cart and sum them up
        public int GetCartItemsCount()
        {
            var cartItems = _context.CartItems
                .Where(cartItem => cartItem.ShoppingCartId == this._shoppingCartId);
            int count = cartItems.Sum(cartItem => cartItem.Count);
            return count;
        }

        public decimal GetTotal()
        {
            return _context.CartItems
                .Where(cartItem => cartItem.ShoppingCartId == this._shoppingCartId)
                .Sum(cartItem => cartItem.Count * cartItem.Album.Price);
        }

        /// <summary>
        /// Create an order from current shopping cart
        /// </summary>
        /// <param name="order">an order which has been created from database, aka, has valid Id</param>
        /// <returns>return newly created order's OrderId as a confirmation number</returns>
        public int CreateOrderDetails(Order order)
        {            
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            foreach(var cartItem in cartItems)
            {
                var orderDetail = new OrderDetail {
                    AlbumId = cartItem.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = cartItem.Album.Price,
                    Quantity = cartItem.Count
                };
                orderTotal += (cartItem.Count * cartItem.Album.Price);
                _context.OrderDetails.Add(orderDetail);                
            }

            order.Total = orderTotal;
            _context.Orders.Update(order);

            _context.SaveChanges();
            // Empty the shopping cart
            EmptyCart();

            return order.OrderId;

        }

        /// <summary>
        /// When a user has logged in, migrate shopping cart to be associated with their user name
        /// </summary>
        /// <param name="userName"></param>
        public void MigrateCart(string userName)
        {
            var cartItems = _context.CartItems.Where(c => c.ShoppingCartId == _shoppingCartId);

            foreach (var cartItem in cartItems)
            {
                cartItem.ShoppingCartId = userName;
                _context.Update(cartItem);
            }
            _context.SaveChanges();
        }
    }
}
