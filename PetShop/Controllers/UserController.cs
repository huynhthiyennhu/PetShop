using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PetShop.data;
using PetShop.Models;
using PetShop.ViewModles;
using PetShop.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace PetShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            db = dbContext;
        }


        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Hoặc ghi vào log
                }
                return View(model);
            }

            // Kiểm tra xem tên đăng nhập đã tồn tại hay chưa
            if (db.Users.Any(u => u.UserName == model.UserName))
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                return View(model);
            }

            model.UserId = Guid.NewGuid().ToString();
            var user = _mapper.Map<User>(model);
            user.PasswordHash = MyUtil.GenerateRandomKey();
            user.PasswordHash = model.Password.ToMd5Hash(user.PasswordHash);

            db.Users.Add(user);
            db.SaveChanges();

            var role = db.Roles.FirstOrDefault(r => r.RoleName == "User");
            if (role == null)
            {
                ModelState.AddModelError("", "Role 'User' not found.");
                return View(model);
            }

            UserRole userRole = new UserRole()
            {
                UserId = user.UserId,
                RoleId = role.RoleId,
                User = user,
                Role = role
            };

            db.UserRoles.Add(userRole);
            db.SaveChanges();

            return RedirectToAction("Login");
        }






        #endregion
        #region Login in
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                // Tìm người dùng trong cơ sở dữ liệu
                var user = db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                    .SingleOrDefault(u => u.UserName == model.UserName);

                if (user == null || user.PasswordHash == model.Password.ToMd5Hash(user.PasswordHash))
                {
                    ModelState.AddModelError("Error", "Thông tin đăng nhập không chính xác");
                    return View(model);
                }

                // Tạo claims cho người dùng
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("UserID", user.UserId),
        };

                
                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);


                TempData["SuccessMessage"] = "Đăng nhập thành công!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }






        #endregion

        [Authorize]
        public IActionResult Profile()
        {
            // Lấy UserId từ Claims
            var userId = User.FindFirst("UserID").Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = db.Users.SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            // Chuyển đổi User thành RegisterVM để sử dụng trong View
            var model = _mapper.Map<RegisterVM>(user);

            return View(model);
        }



        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
