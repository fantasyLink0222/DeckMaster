using DeckMaster.Data;
using DeckMaster.Models;
using DeckMaster.ViewModels;

namespace DeckMaster.Repositories
{
    public class CartRepo
    {

        private readonly ApplicationDbContext _context;
        public CartRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(int id, string sessionId)
        {
            Cart cart = new Cart()
            {
                CartItemId = id,
                SessionId = sessionId,
                Quantity = 1,
            };
            _context.Cart.Add(cart);
            _context.SaveChanges();
            return cart.CartItemId;
        }




        public Cart Find(int id, string sessionId)
        {
            var cartItem = _context.Cart.Where(
               c => c.SessionId == sessionId
               && c.CartItemId == id).FirstOrDefault();
            return cartItem;
        }

        public CartVM GetDetail(int id)
        {
            var cartItem = _context.Cart.Where(c => c.CartItemId == id).FirstOrDefault();
            var book = _context.Products.Where(b => b.ID== cartItem.Product.ID).FirstOrDefault();
            CartVM cartVM = new CartVM()
            {
                CartItemId = cartItem.CartItemId,
                SessionId = cartItem.SessionId,
                
                Price = decimal.Parse(cartItem.Product.Price),
                Quantity = cartItem.Quantity,
              
            };
            return cartVM;
        }
        public IQueryable<CartVM> GetLists(string sessionId)
        {
            var query = from c in _context.Cart
                        from p in _context.Products
                        where c.SessionId == sessionId
                        where p.ID == c.Product.ID
                        
                        select new CartVM()
                        {
                            CartItemId = c.CartItemId,
                            SessionId = c.SessionId,
                        
                            Price = decimal.Parse(p.Price),
                            Quantity = c.Quantity,
                            
                        };
            return query;
        }
        public int GetTotalItems(string sessionId)
        {
            var query = GetLists(sessionId);
            int totalItems = 0;
            foreach (var item in query)
            {
                totalItems += item.Quantity;
            }
            return totalItems;
        }
        public decimal GetSubTotal(string sessionId)
        {
            var query = GetLists(sessionId);
            decimal subTotal = 0;
            foreach (var item in query)
            {
                subTotal += item.Quantity * item.Price;
            }
            return subTotal;
        }
        public Cart GetCartItem(int id)
        {
            Cart cart = _context.Cart
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
            var query = from c in _context.Cart
                        where c.SessionId == sessionId
                        select c;
            if (query != null)
            {
                foreach (var item in query)
                {
                    _context.Cart.Remove(item);
                }
                _context.SaveChanges();
            }


        }
        public Cart increaseQuantity(int id)
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
        public Cart decreaseQuantity(int id)
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

