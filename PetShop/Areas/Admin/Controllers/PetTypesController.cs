using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.data;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/PetTypes")]
    [Authorize(Roles = "Admin")]
    public class PetTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var petTypes = await _context.PetTypes.ToListAsync();
            return View(petTypes);
        }

        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes
                .FirstOrDefaultAsync(m => m.PetTypeId == id);
            if (petType == null)
            {
                return NotFound();
            }

            return View(petType);
        }


        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetTypeId,TypeName,Description")] PetType petType)
        {
            if (ModelState.IsValid)
            {
                petType.PetTypeId = Guid.NewGuid().ToString();
                _context.Add(petType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petType);
        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes.FindAsync(id);
            if (petType == null)
            {
                return NotFound();
            }
            return View(petType);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PetTypeId,TypeName,Description")] PetType petType)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPetType = await _context.PetTypes.FindAsync(petType.PetTypeId);
                    if (existingPetType == null)
                    {
                        return NotFound();
                    }

                    // Chỉ cập nhật các thuộc tính cần thiết
                    existingPetType.TypeName = petType.TypeName;
                    existingPetType.Description = petType.Description;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetTypeExists(petType.PetTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
            }
            return View(petType);
        }

        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petType = await _context.PetTypes
                .FirstOrDefaultAsync(m => m.PetTypeId == id);
            if (petType == null)
            {
                return NotFound();
            }

            return View(petType);
        }

        [Route("Delete")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var petType = await _context.PetTypes.FindAsync(id);
            if (petType == null)
            {
                return NotFound();
            }

            // Kiểm tra có sản phẩm liên kết không
            var hasPets = await _context.Pets.AnyAsync(p => p.PetTypeId == id);
            if (hasPets)
            {
                ModelState.AddModelError("", "This pet type cannot be deleted because there is an associated product.");
                return View("Delete", petType); // Trở lại view xóa với thông báo lỗi
            }

            _context.PetTypes.Remove(petType);
            await _context.SaveChangesAsync();

            // Thông báo xóa thành công
            TempData["SuccessMessage"] = "The pet type has been successfully deleted.";
            return RedirectToAction(nameof(Index));
        }



        private bool PetTypeExists(string id)
        {
            return _context.PetTypes.Any(e => e.PetTypeId == id);
        }
    }
}
