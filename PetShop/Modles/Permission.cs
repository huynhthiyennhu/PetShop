using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Permission
    {
        [Key]
        public string PermissionId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; }

        public string Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
