﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;

namespace FPTJOB.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly DBMyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilesController(DBMyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profile.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        public IActionResult Create()
        {
            ViewBag.UserId = User.Identity.Name;
            return View();
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,FullName,Address,Skill,Education,MyFile,ImageFile")] Profile profile)
        {
            ModelState.Remove("MyFile");
            if (ModelState.IsValid)
            {
                string uniqueFileName = GetUniqueFileName(profile.ImageFile.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profile.ImageFile.CopyToAsync(fileStream);
                }
                profile.MyFile = uniqueFileName;

                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,FullName,Address,Skill,Education,MyFile, ImageFile")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }
            ModelState.Remove("MyFile");
            ModelState.Remove("ImageFile");
            if (ModelState.IsValid)
            {
                try
                {
                    if (profile.ImageFile != null)
                    {
                        string uniqueFileName = GetUniqueFileName(profile.ImageFile.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await profile.ImageFile.CopyToAsync(fileStream);
                        }
                        profile.MyFile = uniqueFileName;
                    }
                    _context.Update(profile);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profile.FindAsync(id);
            if (profile != null)
            {
                _context.Profile.Remove(profile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }
    }
}
