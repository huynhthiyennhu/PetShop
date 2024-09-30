using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class OrderItem
    {
        [Key]
        public string OrderItemId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string OrderId { get; set; }

        [Required]
        public Order Order { get; set; }

        [Required]
        public string PetId { get; set; }

        //[Required]
        public Pet Pet { get; set; }

        [Required]
        [Range(1, 1, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
