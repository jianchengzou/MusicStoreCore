using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private MusicStoreDbContext _context;

        public CartSummaryViewComponent(MusicStoreDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var cart = ShoppingCart.GetCart(_context, this.HttpContext);

            ViewData["CartCount"] = cart.GetCartItemsCount();
            return View("CartSummary", cart.GetCartItemsCount());
        }
    }
}
