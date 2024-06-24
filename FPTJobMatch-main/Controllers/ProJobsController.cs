using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;

namespace FPTJOB.Controllers
{
    public class ProJobsController : Controller
    {
        private readonly DBMyContext _context;

        public ProJobsController(DBMyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var dBMyContext = _context.ProJob.Include(p => p.Job).Include(p => p.Profile).Where(p=>p.JobId == id);
            return View(await dBMyContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob
                .Include(p => p.Job)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proJob == null)
            {
                return NotFound();
            }

            return View(proJob);
        }


        public IActionResult Create(int id)
        {
            ProJob pj = new ProJob();
            pj.JobId = id;
            pj.RegDate = DateTime.Now;
            pj.ProfileId = _context.Profile.Where(p => p.UserId == User.Identity.Name).FirstOrDefault().Id;
            _context.Add(pj);
            _context.SaveChanges();

            return RedirectToAction("ListJob", "Jobs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegDate,JobId,ProfileId")] ProJob proJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }

        // GET: ProJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob.FindAsync(id);
            if (proJob == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegDate,JobId,ProfileId")] ProJob proJob)
        {
            if (id != proJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProJobExists(proJob.Id))
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
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }

        // GET: ProJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob
                .Include(p => p.Job)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proJob == null)
            {
                return NotFound();
            }

            return View(proJob);
        }

        // POST: ProJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proJob = await _context.ProJob.FindAsync(id);
            if (proJob != null)
            {
                _context.ProJob.Remove(proJob);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProJobExists(int id)
        {
            return _context.ProJob.Any(e => e.Id == id);
        }
    }
}
