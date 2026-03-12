using DentalClinicWebsite.Data;
using DentalClinicWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DentalClinicWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new Appointment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction("Index");
            }

            appointment.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            try
            {
                var logicAppUrl = _configuration["LogicApp:AppointmentUrl"];

                using var httpClient = new HttpClient();

                var payload = new
                {
                    fullName = appointment.FullName,
                    email = appointment.Email,
                    phone = appointment.Phone,
                    service = appointment.Service,
                    preferredDate = appointment.PreferredDate,
                    preferredTime = appointment.PreferredTime,
                    message = appointment.Message,
                    createdAt = appointment.CreatedAt
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(logicAppUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Logic App call failed with status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Logic App.");
            }

            TempData["Success"] = "Your appointment has been booked successfully!";
            return RedirectToAction("Index");
        }
    }
}