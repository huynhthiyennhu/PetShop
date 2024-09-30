using PetShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class OrderItem
    {

        public string PetId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
