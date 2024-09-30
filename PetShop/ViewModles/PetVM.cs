using PetShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShop.ViewModles
{
    public class PetVM
    {
        public string PetId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

       
        public string Breed { get; set; }

        
        public string Age { get; set; }
       
        public string ImageUrl { get; set; }
        public string PetTypeId { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> PetImages { get; set; } = new List<string>();
        public List<Pet> SimilarPets { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
       


    }
}
