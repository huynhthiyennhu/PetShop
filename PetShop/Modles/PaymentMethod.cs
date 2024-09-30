using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class PaymentMethod
    {
        [Key]
        public string PaymentMethodId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string MethodName { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public PaymentType PaymentType { get; set; }
    }

    public enum PaymentType
    {
        CreditCard,
        PayPal,
        DebitCard
    }
}
