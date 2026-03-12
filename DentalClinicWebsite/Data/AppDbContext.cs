using DentalClinicWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
    }
}