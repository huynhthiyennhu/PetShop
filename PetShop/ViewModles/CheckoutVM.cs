using PetShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class CheckoutVM
    {
        public string? CustomerName { get; set; } 

        public string? PhoneNumber { get; set; } 

        [Required]
        public string? Address { get; set; }
        public bool UseAccountInfo { get; set; }
        

    }

}
