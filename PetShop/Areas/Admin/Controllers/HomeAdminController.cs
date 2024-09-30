using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.data;
using PetShop.Models;
using PetShop.ViewModles;
using X.PagedList;


namespace PetShop.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly ApplicationDbContext db;
        public HomeAdminController(ApplicationDbContext context)
        {
            db = context;
        }
        [Route("")]
        [Route("index")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("productcategory")]
        public IActionResult productcategory()
        {
            var productcategory = db.PetTypes.ToList();
            return View(productcategory);


        }


        [Route("CreateproductcatePetory")]
        [HttpGet]
        public IActionResult CreateproductcatePetory()
        {
            return View();
        }

        [Route("CreateproductcatePetory")]
        [HttpPost]
        public IActionResult CreateproductcatePetory(PetType petType)
        {
            if (ModelState.IsValid)
            {
                db.Add(petType);
                db.SaveChanges();
                return RedirectToAction("productcategory");
            }
            return View(petType);
        }

        [Route("Pets")]
        public IActionResult Pets(int? page = 1)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            var pets = db.Pets.AsNoTracking().OrderBy(x => x.Name);
            var pagedList = pets.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

    }
}
