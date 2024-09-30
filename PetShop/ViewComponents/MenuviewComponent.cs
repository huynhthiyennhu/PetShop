using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.data;
using PetShop.ViewModles;

namespace PetShop.ViewComponents
{
    public class MenuviewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        public MenuviewComponent(ApplicationDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.PetTypes.Select(pettype => new Menu
            {
                PetTypeId = pettype.PetTypeId,
                TypeName = pettype.TypeName,
                Quantity = pettype.Pets.Count
            });
            return View(data); 
        }
    }
}
