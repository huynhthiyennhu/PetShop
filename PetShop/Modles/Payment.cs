using System;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Payment
    {
        [Key]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string OrderId { get; set; }

        [Required]
        public Order Order { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public int PaymentMethod { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        public int TransactionId { get; set; } // ID giao dịch từ dịch vụ thanh toán
    }
}
