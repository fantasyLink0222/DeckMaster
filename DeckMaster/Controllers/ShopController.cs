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
using System.Security.Claims;

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

        // GET: Products
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         ;
            var productRepo = new ProductRepo(_context);
            var productVMs = productRepo.GetProductVMs();
            var cartVMs = productVMs.Select(p => new CartVM
            {
                Product = p,
                Quantity = 0,
                Price = 0
            }).ToList();

            return View(cartVMs);
        }

        private Cart GetOrCreateCart(string userId)
        {


            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       

        public ActionResult Home()
        {
            return RedirectToAction("Index", "Shop");
        }

        public ActionResult Details(int cartId)
        {
            var cartRepo = new CartRepo(_context);
            var cartVM = cartRepo.GetDetail(cartId);
            return View(cartVM);
        }

        public ActionResult Delete(int cartId)
        {
            try
            {
                var cartRepo = new CartRepo(_context);
                cartRepo.Delete(cartId);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
            }
            return RedirectToAction("Index", "Shop", new { message = ViewData["Message"] });
        }

        public ActionResult IncreaseOne(int cartId)
        {
            var cartRepo = new CartRepo(_context);
            cartRepo.IncreaseQty(cartId);
            return RedirectToAction("Index", "Shop");
        }

        public ActionResult DecreaseOne(int cartId)
        {
            var cartRepo = new CartRepo(_context);
            cartRepo.DecreaseQty(cartId);
            return RedirectToAction("Index", "Shop");
        }

        // Other methods...



        public IActionResult PayPalConfirmation(PayPalConfirmationModel payPalConfirmationModel)
        {
            return View(payPalConfirmationModel);
        }



    }
}
