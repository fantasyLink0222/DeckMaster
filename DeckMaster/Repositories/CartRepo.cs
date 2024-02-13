using DeckMaster.Data;
using DeckMaster.Models;
using DeckMaster.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeckMaster.Repositories
{
    public class CartRepo
    {

        private readonly ApplicationDbContext _context;
        public CartRepo(ApplicationDbContext context)
        {
            _context = context;
        }

     
       
        public int Add(int id)
        {
            Cart cart = new Cart()
            {
                CartItemId = id,
           
                Quantity = 1,
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart.CartItemId;
        }




        public Cart Find(int id)
        {
            var cartItem = _context.Carts.Where(c =>c.CartItemId == id).FirstOrDefault();
            return cartItem;
        }

        public CartVM GetDetail(int id)
        {

            ProductRepo productRepo = new ProductRepo(_context);
            var productVM = productRepo.GetProduct(id);
            var cartItem = _context.Carts.Include(c => c.Product)
                                 .FirstOrDefault(c => c.CartItemId == id);

            if (cartItem == null || cartItem.Product == null)
            {
                // Handle the case where the cart item or product doesn't exist
                return null; // or handle it as per your application's requirement
            }

            CartVM cartVM = new CartVM()
            {
                CartItemId = cartItem.CartItemId,
                Price = decimal.Parse(cartItem.Product.Price), // Ensure Price is a string, else directly assign
                Quantity = cartItem.Quantity,
                Product = productVM
               
            };

            return cartVM;
        }
        public IQueryable<CartVM> GetLists()
        {
            ProductRepo productRepo = new ProductRepo(_context);
            var productVMs = productRepo.GetProductVMs();

            var query = from c in _context.Carts
                        from p in productVMs

                        where p.ID == c.Product.ID
                        
                        select new CartVM()
                        {
                            CartItemId = c.CartItemId,
                            Product = p,
                            Price =p.Price,
                            Quantity = c.Quantity,
                            
                        };
            return query;
        }
        public int GetTotalItems()
        {
            var query = from c in _context.Carts
                        select c;
            int totalItems = 0;
            foreach (var item in query)
            {
                totalItems += item.Quantity;
            }
            return totalItems;
        }
        public decimal GetSubTotal()
        {
            var query = GetLists();
            decimal subTotal = 0;
            foreach (var item in query)
            {
                subTotal += item.Quantity * item.Price;
            }
            return subTotal;
        }
        public Cart GetCartItem(int id)
        {
            Cart cart = _context.Carts
                       .Where(e => e.CartItemId == id).FirstOrDefault();
            return cart;
        }
        public bool Delete(int id)
        {
            var cart = GetCartItem(id);
            if (cart != null)
            {
                _context.Remove(cart);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EmptyCarts(string sessionId)
        {
            var query = from c in _context.Carts
          
                        select c;
            if (query != null)
            {
                foreach (var item in query)
                {
                    _context.Carts.Remove(item);
                }
                _context.SaveChanges();
            }


        }
        public Cart IncreaseQty(int id)
        {
            var cartItem = GetCartItem(id);
            var cart = GetDetail(id);
            if (cartItem.Quantity < cart.Quantity)
            {
                cartItem.Quantity += 1;
                _context.SaveChanges();
            }
            return cartItem;
        }
        public Cart DecreaseQty(int id)
        {
            var cartItem = GetCartItem(id);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
                _context.SaveChanges();
            }
            return cartItem;
        }

   

    }
}

