using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
   
        public class CartVM
        {
            [Key]
            public int CartItemId { get; set; }
            public string SessionId { get; set; }
            
            public ProductVM Product { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }

        }
    
}
