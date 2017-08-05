using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.Models;
using MusicStoreCore.ViewModel;
using Microsoft.EntityFrameworkCore;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreCore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private MusicStoreDbContext _context;

        public ShoppingCartController(MusicStoreDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var shoppingCart = ShoppingCart.GetCart(_context, this.HttpContext);

            var viewModel = new ShoppingCartViewModel {
                CartItems = shoppingCart.GetCartItems(),
                CartTotal = shoppingCart.GetTotal()
            };

            return View(viewModel);
        }

        public IActionResult AddToCart(int albumId)
        {
            var addedAlbum = _context.Albums.SingleOrDefault(a => a.AlbumId == albumId);

            if (addedAlbum != null)
            {
                var cart = ShoppingCart.GetCart(_context, this.HttpContext);
                cart.AddToCart(addedAlbum);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int recordId)
        {
            var cart = ShoppingCart.GetCart(_context, this.HttpContext);

            string albumName = _context.CartItems.Include(nameof(Album)).SingleOrDefault(c => c.RecordId == recordId)?.Album.Title;

            int itemCount = cart.RemoveFromCart(recordId);

            var results = new ShoppingCartRemoveViewModel {
                Message = System.Net.WebUtility.HtmlEncode(albumName) + " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCartItemsCount(),
                ItemCount = itemCount,
                DeleteId = recordId
            };

            return Json(results);
        }

    }
}
