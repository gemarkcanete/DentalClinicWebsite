using System.ComponentModel.DataAnnotations;

namespace DentalClinicWebsite.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Service { get; set; }

        [Required]
        public string? PreferredDate { get; set; }

        [Required]
        public string? PreferredTime { get; set; }

        public string? Message { get; set; }
        public string? CreatedAt { get; set; }
    }
}