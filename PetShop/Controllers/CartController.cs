using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.data;

using PetShop.Helpers;
using PetShop.Models;
using PetShop.ViewModles;

namespace PetShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;
        public CartController(ApplicationDbContext context)
        {
            db = context;
        }
        
        public List<ViewModles.OrderItem> Cart => HttpContext.Session.Get<List<ViewModles.OrderItem>>(MySetting.CART_KEY) ?? new List<ViewModles.OrderItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }



        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(string id)
        {
            var giohang = HttpContext.Session.Get<List<ViewModles.OrderItem>>(MySetting.CART_KEY) ?? new List<ViewModles.OrderItem>();
            var item = giohang.SingleOrDefault(p => p.PetId == id);

            if (item == null)
            {
                var pet = db.Pets.SingleOrDefault(p => p.PetId == id);
                if (pet == null)
                {
                    return Json(new { success = false, message = $"No item found with code {id}" });
                }

                item = new ViewModles.OrderItem
                {
                    PetId = pet.PetId,
                    Name = pet.Name,
                    Price = pet.Price,
                    ImageUrl = pet.ImageUrl,
                };

                giohang.Add(item);
            }

            HttpContext.Session.Set(MySetting.CART_KEY, giohang);

            var totalQuantity = giohang.Count;
            var totalValue = giohang.Sum(p => p.Price); // Tính tổng giá trị giỏ hàng

            // Trả về danh sách các mặt hàng trong giỏ hàng
            var cartItems = giohang.Select(i => new
            {
                PetId = i.PetId,
                Name = i.Name,
                ImageUrl = i.ImageUrl,
                Price = i.Price
            }).ToList();

            return Json(new { success = true, message = "Item added to cart successfully!", totalQuantity, totalValue, items = cartItems });
        }









        [Authorize]
       
        public IActionResult RemoveCart(string id)
        {
            var giohang = Cart;
            var item = giohang.SingleOrDefault(p => p.PetId == id);
            if(item != null)
            {
                giohang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);
            }
            return RedirectToAction("Index");
        }


        [Authorize]
       
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
           

            return View(Cart);
        }

        [Authorize]
        [HttpPost]
       
        public IActionResult Checkout(CheckoutVM model)
        {
            if (Cart.Count == 0)
            {
                return Redirect("/"); 
            }

            if (ModelState.IsValid)
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAM_CUSTOMERID)?.Value;
                var khachHang = db.Users.SingleOrDefault(kh => kh.UserId == customerId);

                var hoadon = new Order
                {
                    UserId = customerId,
                    CustomerName = model.CustomerName ?? khachHang?.FullName,
                    ShippingAddress = model.Address ?? khachHang?.Address,
                    PhoneNumber = model.PhoneNumber ?? khachHang?.PhoneNumber,
                    OrderDate = DateTime.Now,
                };

                db.Database.BeginTransaction();
                try
                {
                   
                    db.Add(hoadon);
                    db.SaveChanges();

                  
                    foreach (var item in Cart)
                    {
                        
                        var pet = db.Pets.SingleOrDefault(p => p.PetId == item.PetId);
                        if (pet != null)
                        {
                            pet.IsAvailable = false; 
                            db.Update(pet); 
                        }

                       
                        var orderItem = new Models.OrderItem
                        {
                            OrderId = hoadon.OrderId,
                            Quantity = 1,
                            Price = item.Price,
                            PetId = item.PetId
                        };
                        db.Add(orderItem);
                    }
                    db.SaveChanges(); 

                   
                    HttpContext.Session.Set<List<ViewModles.OrderItem>>(MySetting.CART_KEY, new List<ViewModles.OrderItem>());

                    db.Database.CommitTransaction();
                    return View("Success"); 
                }
                catch
                {
                    db.Database.RollbackTransaction(); 
                    ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                }
            }

            return View(Cart); 
        }

        [Authorize]
        public JsonResult GetCartItems()
        {
            var giohang = Cart;
            var cartItems = giohang.Select(item => new
            {
                item.PetId,
                item.Name,
                item.Price,
                item.ImageUrl
            }).ToList();

            return Json(cartItems);
        }


    }
    }
