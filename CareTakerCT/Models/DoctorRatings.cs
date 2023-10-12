using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareTakerCT.Models
{
    public class DoctorRatings
    {
        public int Id { get; set; }

        [Display(Name = "Rating")]
        [Range(1, 10, ErrorMessage = "The value must be between 1 and 10.")]
        public int Value { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }

        public virtual ApplicationUser Doctor { get; set; }
    }
}