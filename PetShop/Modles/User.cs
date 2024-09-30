using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class User
    {
        [Key]
        public string? UserId { get; set; } =Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }


}
