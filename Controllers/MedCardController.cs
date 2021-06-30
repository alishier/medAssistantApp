using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medAssisTantApp.Data;
using medAssisTantApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace medAssisTantApp.Controllers
{
    public class MedCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public MedCardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MedCard
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var medCard = (from medcard in _context.MedCard
                           join patient in _context.Patient on medcard.PatientId equals patient.Id
                           join doctor in _context.Doctor on patient.DoctorId equals doctor.Id
                           where doctor.UserId == userEmail
                           select medcard);
            //var applicationDbContext = _context.MedCard.Include(m => m.Patient);
            return View(await medCard.ToListAsync());
        }

        // GET: MedCard/Details/5
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            var userEmail = User.Identity.Name;

            var doctor = _context.Doctor.Where(d => d.UserId == userEmail).First();
            var patientIdList = _context.Patient.Where(p => p.DoctorId == doctor.Id).Select(p => p.Id);
            Console.WriteLine("new line ---------------------------------" + patientIdList.Count());
            ViewData["PatientId"] = new SelectList(patientIdList);
            return View();
        }

        // POST: MedCard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
