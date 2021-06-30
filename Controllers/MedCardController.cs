using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medAssisTantApp.Data;
using medAssisTantApp.Models;

namespace medAssisTantApp.Controllers
{
    public class MedCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedCardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedCard
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedCard.Include(m => m.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedCard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medCard = await _context.MedCard
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medCard == null)
            {
                return NotFound();
            }

            return View(medCard);
        }

        // GET: MedCard/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Id");
            return View();
        }

        // POST: MedCard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Complain,Description,Diagnosis,Instructions,PatientId,EnrollmentDate")] MedCard medCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Id", medCard.PatientId);
            return View(medCard);
        }

        // GET: MedCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medCard = await _context.MedCard.FindAsync(id);
            if (medCard == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Id", medCard.PatientId);
            return View(medCard);
        }

        // POST: MedCard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Complain,Description,Diagnosis,Instructions,PatientId,EnrollmentDate")] MedCard medCard)
        {
            if (id != medCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedCardExists(medCard.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Id", medCard.PatientId);
            return View(medCard);
        }

        // GET: MedCard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medCard = await _context.MedCard
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medCard == null)
            {
                return NotFound();
            }

            return View(medCard);
        }

        // POST: MedCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medCard = await _context.MedCard.FindAsync(id);
            _context.MedCard.Remove(medCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedCardExists(int id)
        {
            return _context.MedCard.Any(e => e.Id == id);
        }
    }
}
