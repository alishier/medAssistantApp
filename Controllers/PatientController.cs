using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medAssisTantApp.Data;
using medAssisTantApp.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace medAssisTantApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: Patient
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var userEmail = User.Identity.Name;        

            var patients = (from patient in _context.Patient 
                           join doctor in _context.Doctor on patient.DoctorId equals doctor.Id
                           where doctor.UserId == userEmail
                           select patient);

            //var applicationDbContext = _context.Patient.Include(p => p.Doctor);
            return View(await patients.ToListAsync());
        }

        // GET: Patient/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id");
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Gender,DoctorId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var doctor = (from d in _context.Doctor where d.UserId == userEmail select d).First();
                patient.Doctor = doctor;
                patient.DoctorId = doctor.Id;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id", patient.DoctorId);
            return View(patient);
        }

        // GET: Patient/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id", patient.DoctorId);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Gender,DoctorId")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id", patient.DoctorId);
            return View(patient);
        }

        // GET: Patient/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
    }
}
