using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class LoginVM
    {
        [Display(Name ="User Name")]
        [Required(ErrorMessage = "No login name entered")]
        [MaxLength(20,ErrorMessage = "maximum 20 characters")]
        public string UserName { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage = "No password entered")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
