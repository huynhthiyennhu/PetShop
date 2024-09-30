using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "This field is required.")]
       
        
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = "UserName")]
        [Required(ErrorMessage ="*")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name ="Email")]
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Display(Name ="FullName")]
        [Required(ErrorMessage = "*")]
        public string FullName { get; set; }

        [Display(Name ="Address")]
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
    }
}
