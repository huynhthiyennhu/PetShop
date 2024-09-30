using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Pet
    {
        [Key]
        public string? PetId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public string Breed { get; set; }

        [Required]
        public string Age { get; set; }

        
        public string? ImageUrl { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        [Required]
        public string? PetTypeId { get; set; }

        public ICollection<PetImage>? PetImages { get; set; }=new List<PetImage>();
    }
}
