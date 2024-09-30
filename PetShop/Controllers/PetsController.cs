using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.data;
using PetShop.Models;
using PetShop.ViewModles;
using System.Drawing;

namespace PetShop.Controllers
{
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext db;

        public PetsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index(string PetTypeID, int page = 1)
        {
            var pets = db.Pets.Where(p => p.IsAvailable).AsQueryable();

            if (!string.IsNullOrEmpty(PetTypeID))
            {
                pets = pets.Where(p => p.PetTypeId == PetTypeID);
            }

            var pageSize = 6; 
            var totalCount = pets.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var result = pets.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(p => new PetVM
                             {
                                 PetId = p.PetId,
                                 Name = p.Name,
                                 Price = p.Price,
                                 Description = p.Description ?? "",
                                 Breed = p.Breed,
                                 Age = p.Age,
                                 ImageUrl = p.ImageUrl ?? "",
                                 PetTypeId = p.PetTypeId,
                                 CurrentPage = page,
                                 TotalPages = totalPages
                             })
                             .ToList();

            return View(result);
        }


        public IActionResult Search(string query, int page = 1)
        {
            int pageSize = 6; 
            var pets = db.Pets.Where(p => p.IsAvailable).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                pets = pets.Where(p => p.Name.Contains(query));
            }

            var totalCount = pets.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var result = pets.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(p => new PetVM
                             {
                                 PetId = p.PetId,
                                 Name = p.Name,
                                 Price = p.Price,
                                 Description = p.Description ?? "",
                                 Breed = p.Breed,
                                 Age = p.Age,
                                 ImageUrl = p.ImageUrl ?? "",
                                 PetTypeId = p.PetTypeId,
                                 CurrentPage = page,
                                 TotalPages = totalPages
                             })
                             .ToList();

            

            return View(result);
        }

        public IActionResult Detail(string id)
        {
            

            var pet = db.Pets.Include(p => p.PetImages).SingleOrDefault(p => p.PetId == id);

            if (pet == null)
            {
                TempData["Message"] = "No product information found.";
                return Redirect("/404");
            }

           
            var similarPets = db.Pets
                .Where(p => p.PetTypeId == pet.PetTypeId && p.PetId != pet.PetId) 
                .Include(p => p.PetImages).Take(5).ToList();
                
                

          
            var petVM = new PetVM
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Description = pet.Description,
                Price = pet.Price,
                Breed = pet.Breed,
                Age = pet.Age,
                ImageUrl = pet.ImageUrl,
                IsAvailable = pet.IsAvailable,
                PetImages = pet.PetImages.Select(pi => pi.ImageUrl).ToList(),
                SimilarPets = similarPets.Select(p => new Pet
                {
                    PetId = p.PetId,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price
                }).ToList()
            };

            return View(petVM);
        }




    }
}
