using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.data;
using PetShop.Models;
using PetShop.ViewModles;
using System.Linq;

namespace PetShop.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var petTypes = _context.PetTypes.ToList();

            var result = petTypes
                .Select(pt => _context.Pets
                    .Where(p => p.PetTypeId == pt.PetTypeId && p.IsAvailable )
                    .FirstOrDefault())
                .Where(p => p != null) 
                .Select(p => new PetVM
                {
                    PetId = p.PetId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description ?? "",
                    Breed = p.Breed,
                    Age = p.Age,
                    ImageUrl = p.ImageUrl ?? "",
                    PetTypeId = p.PetTypeId
                })
                .ToList();

            return View(result);
        }


        [Route("/404")]
        public IActionResult Error404()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
