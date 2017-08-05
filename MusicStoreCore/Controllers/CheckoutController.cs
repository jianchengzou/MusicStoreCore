using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MusicStoreCore.Models;
using MusicStoreCore.ViewModel;

namespace MusicStoreCore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private MusicStoreDbContext _context;
        public const string PromoCode = "FREE";

        public CheckoutController(MusicStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddressAndPayment(AddressAndPaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order {
                    OrderDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    Phone = model.Phone,
                    Email = model.EmailAddress                    
                };

                if (string.Equals(model.PromoCode, PromoCode ,StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(model);
                }
                else
                {
                    try
                    {
                        _context.Orders.Add(order);
                        _context.SaveChanges();

                        var cart = ShoppingCart.GetCart(_context, this.HttpContext);
                        cart.CreateOrderDetails(order);

                        return RedirectToAction("Complete", new { orderId = order.OrderId});
                    }
                    catch(Exception ex)
                    {
                        //error!
                        return View(model);
                    }
                }                
            }

            return View(model);
        }

        public IActionResult Complete(int orderId)
        {
            bool isValid = _context.Orders.Any(o => o.OrderId == orderId &&
                                                o.UserName == User.Identity.Name);

            if (isValid)
            {
                return View(orderId);
            }
            return View("Error");//The Error view was automatically created for us in the /Views/Shared folder when we began the project
        }
    }
}