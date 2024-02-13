using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeckMaster.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int Quantity { get; set; }

        public decimal CartPrice { get; set; }
        public virtual Product Product { get; private set; }
    }
}
