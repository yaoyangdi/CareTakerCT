using System;
using System.ComponentModel.DataAnnotations;

namespace CareTakerCT.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Booking Time")]
        public DateTime BookTime { get; set; }

        [Required]
        public string Description { get; set; }
        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }

        [Required]
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public string DoctorId { get; set; }
        public virtual ApplicationUser Doctor { get; set; } 


    }
}