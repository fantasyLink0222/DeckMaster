using DeckMaster.Models;
using System;
using DeckMaster.Data;

namespace DeckMaster.Models
{
    public class ShoppingCart
    {
        public List<Cart> Items { get; set; } = new List<Cart>();
        public void AddItem(Product product, int quantity)
        {
            Cart item = Items
                .Where(p => p.Product.ID == product.ID)
                .FirstOrDefault();
            if (item == null)
            {
                Items.Add(new Cart
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }
        public void RemoveItem(Product product)
        {
            Items.RemoveAll(l => l.Product.ID == product.ID);
        }
        public decimal ComputeTotalValue()
        {

            return Items.Sum(e => decimal.Parse(e.Product.Price) * e.Quantity);
        }
        public void Clear()
        {
            Items.Clear();
        }
    }
}
