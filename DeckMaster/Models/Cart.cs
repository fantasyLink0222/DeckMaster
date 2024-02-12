using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace DeckMaster.Models
{
    public class Cart
    {
        [Key]
        public int CartItemId { get; set; }

        public string SessionId { get; set; }
        public Product Product { get; set; }

 
        public int Quantity { get; set; }

        public decimal CartPrice { get; set; }
    }
}
