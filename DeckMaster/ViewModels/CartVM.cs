using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
   
        public class CartVM
        {
            
            public int? CartItemId { get; set; }
            
            public required ProductVM Product { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }

        }
    
}
