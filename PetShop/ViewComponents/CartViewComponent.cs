using Microsoft.AspNetCore.Mvc;
using PetShop.Helpers;
using PetShop.ViewModles;

namespace PetShop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var count = HttpContext.Session.Get<List<OrderItem>>(MySetting.CART_KEY) ?? new List<OrderItem>();
            var cartModels = count.Select(item => new CartModel
            {
                Id= item.PetId,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                Price = item.Price
            }).ToList();
            var totalValue = cartModels.Sum(item => item.Total);
            var totalQuantity = cartModels.Count;


            var cartSummary = new CartSummaryModel
            {
                Items = cartModels,
                TotalValue = totalValue,
                TotalQuantity = totalQuantity
            };

            
            return View("CartPanel",cartSummary);
        }
    }
}
