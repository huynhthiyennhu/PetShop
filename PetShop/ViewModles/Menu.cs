using PetShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class Menu
    {
        public string PetTypeId { get; set; }

        
        public string TypeName { get; set; }

        public int Quantity { get; set; }

       
    }
}
