using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeckMaster.Data;
using DeckMaster.Models;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.AspNetCore.Http;
using DeckMaster.Repositories;
using DeckMaster.ViewModels;

namespace DeckMaster.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShopController> _logger;

        public ShopController(ApplicationDbContext context, ILogger<ShopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        const string CARTITEMS = "CartItems";

        public string GetSessionId()
        {
            if (HttpContext.Session.GetString("SessionId") == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
                {

                    HttpContext.Session.SetString("SessionId", HttpContext.User.Identity.Name);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempSessionId = Guid.NewGuid();
                    HttpContext.Session.SetString("SessionId", tempSessionId.ToString());
                }
            }

            return HttpContext.Session.GetString("SessionId");
        }

        // GET: Products
        public IActionResult Index()
        {
            ProductRepo productRepo = new ProductRepo(_context);
            CartRepo cartRepo = new CartRepo(_context); 
            var productVMs = productRepo.GetProductVMs();
            var cartVMs = productVMs.Select(p => new CartVM
            {
                Product = p,
                Quantity = 0,
                Price = 0
            }).ToList();


            return View(cartVMs);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        

        public IActionResult ShopIndex()
        {
            string sessionId = GetSessionId();
            CartRepo cartRepo = new CartRepo(_context);
            var query = cartRepo.GetLists(sessionId);
            var totalItems = cartRepo.GetTotalItems(sessionId);
            var subTotals = cartRepo.GetSubTotal(sessionId);
            HttpContext.Session.SetInt32(CARTITEMS, totalItems);

            ViewData["TotalItems"] = HttpContext.Session.GetInt32(CARTITEMS);
            ViewData["SubTotal"] = Math.Round(subTotals * 1.00m, 2, MidpointRounding.ToEven);
            ViewData["Tax"] = Math.Round(subTotals * 0.12m, 2, MidpointRounding.ToEven);
            ViewData["Total"] = Math.Round(subTotals * 1.12m, 2, MidpointRounding.ToEven);
            return View(query);
        }

        public ActionResult Home()
        {

            return RedirectToAction("Index", "Shop");
        }
        public ActionResult Details(int cartId)
        {
            CartRepo cartRepo = new CartRepo(_context);
            var cartVM = cartRepo.GetDetail(cartId);
            return View(cartVM);
        }

        public ActionResult Delete(int cartId)
        {
            try
            {
                CartRepo cartRepo = new CartRepo(_context);
                cartRepo.Delete(cartId);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
            }
            return RedirectToAction("Index", "Cart", new { message = ViewData["Message"] });
        }
        public ActionResult IncreaseOne(int cartId)
        {
            CartRepo cartRepo = new CartRepo(_context);
            cartRepo.increaseQuantity(cartId);
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DecreaseOne(int cartId)
        {
            CartRepo cartRepo = new CartRepo(_context);
            cartRepo.decreaseQuantity(cartId);
            return RedirectToAction("Index", "Cart");
        }






    }
}
