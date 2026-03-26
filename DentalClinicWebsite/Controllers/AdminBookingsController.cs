using DentalClinicWebsite.Data;
using DentalClinicWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicWebsite.Controllers
{
    public class AdminBookingsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminBookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /AdminBookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Appointments
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(bookings);
        }

        // GET: /AdminBookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: /AdminBookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(appointment);
            }

            var existingAppointment = await _context.Appointments.FindAsync(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.FullName = appointment.FullName;
            existingAppointment.Email = appointment.Email;
            existingAppointment.Phone = appointment.Phone;
            existingAppointment.Service = appointment.Service;
            existingAppointment.PreferredDate = appointment.PreferredDate;
            existingAppointment.PreferredTime = appointment.PreferredTime;
            existingAppointment.Message = appointment.Message;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminBookings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}