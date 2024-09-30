using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class PetType
    {
        [Key]
        public string? PetTypeId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        public string Description { get; set; }

        public ICollection<Pet>? Pets { get; set; }
    }

}
