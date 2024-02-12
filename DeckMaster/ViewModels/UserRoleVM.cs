using System.ComponentModel.DataAnnotations;

namespace DeckMaster.ViewModels
{
    public class UserRoleVM
    {

        [Key] public int? ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
