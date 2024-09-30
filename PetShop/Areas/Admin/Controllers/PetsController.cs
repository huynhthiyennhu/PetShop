using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using PetShop.data;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace PetShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Pets")]
    [Authorize(Roles = "Admin")]
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [Route("Index")]
        public async Task<IActionResult> Index(int? page = 1)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var pets = _context.Pets.AsNoTracking().OrderBy(x => x.Name);
            var pagedList = pets.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

       

        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

       
        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var petTypes = await _context.PetTypes.ToListAsync();
            ViewBag.PetTypeId = new SelectList(petTypes, "PetTypeId", "TypeName");
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Breed,Age,IsAvailable,PetTypeId")] Pet pet, IFormFile[] Images, IFormFile MainImage)
        {
            if (ModelState.IsValid)
            {
                pet.PetId = Guid.NewGuid().ToString();

                
                if (MainImage != null && MainImage.Length > 0)
                {
                    var mainFileName = MainImage.FileName; 

                   
                    var mainFilePath = Path.Combine("wwwroot/images/pet", mainFileName);
                    using (var stream = new FileStream(mainFilePath, FileMode.Create))
                    {
                        await MainImage.CopyToAsync(stream);
                    }

                    pet.ImageUrl = mainFileName; 
                }

                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();

               
                if (Images != null && Images.Length > 0)
                {
                    foreach (var image in Images.Take(3)) 
                    {
                        if (image.Length > 0)
                        {
                            var fileName = image.FileName; 

                           
                            var filePath = Path.Combine("wwwroot/images/pet", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var petImage = new PetImage
                            {
                                PetId = pet.PetId,
                                ImageUrl = fileName 
                            };

                            _context.PetImages.Add(petImage);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var petTypes = await _context.PetTypes.ToListAsync();
            ViewBag.PetTypeId = new SelectList(petTypes, "PetTypeId", "TypeName");
            return View(pet);
        }




        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            var petTypes = await _context.PetTypes.ToListAsync();
            ViewBag.PetTypeId = new SelectList(petTypes, "PetTypeId", "TypeName");
            return View(pet);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit([Bind("PetId,Name,Description,Price,Breed,Age,ImageUrl,IsAvailable,PetTypeId")] Pet pet)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm bản ghi hiện tại
                    var existingPet = await _context.Pets.FindAsync(pet.PetId);
                    if (existingPet == null)
                    {
                        return NotFound();
                    }

                    // Chỉ cập nhật các thuộc tính cần thiết
                    existingPet.Name = pet.Name;
                    existingPet.Description = pet.Description;
                    existingPet.Price = pet.Price;
                    existingPet.Breed = pet.Breed;
                    existingPet.Age = pet.Age;
                    existingPet.IsAvailable = pet.IsAvailable;
                    existingPet.PetTypeId = pet.PetTypeId;
                    // Không thay đổi ImageUrl

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.PetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var petTypes = await _context.PetTypes.ToListAsync();
            ViewBag.PetTypeId = new SelectList(petTypes, "PetTypeId", "TypeName");
            return View(pet);
        }



        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [Route("Delete")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            
            var pet = await _context.Pets.Include(p => p.PetImages)
                                           .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet != null)
            {
                foreach (var image in pet.PetImages)
                {
                    var filePath = Path.Combine("wwwroot/images/pet", image.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                var filePathmain = Path.Combine("wwwroot/images/pet", pet.ImageUrl);
                if (System.IO.File.Exists(filePathmain))
                {
                    System.IO.File.Delete(filePathmain);
                }
                foreach (var image in pet.PetImages)
                {
                    _context.PetImages.Remove(image);
                }

                
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();

                
                TempData["SuccessMessage"] = "The pet has been successfully deleted.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        private bool PetExists(string id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }
    }
}
