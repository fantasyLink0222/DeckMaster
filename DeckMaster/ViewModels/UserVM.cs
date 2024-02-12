using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
    public class UserVM
    {
        [Key]

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
