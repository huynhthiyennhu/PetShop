using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class PetImage
    {
        [Key]
        public string ImageId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string PetId { get; set; }

        [Required]
        public Pet Pet { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        
       
    }
}
