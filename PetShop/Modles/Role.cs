using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Role
    {
        [Key]
        public string RoleId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
