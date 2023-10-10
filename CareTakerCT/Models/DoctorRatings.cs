using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareTakerCT.Models
{
    public class DoctorRatings
    {
        public int Id { get; set; }

        public float Value { get; set; }

        public string Comment { get; set; }

        public string DoctorId { get; set; }

        public virtual ApplicationUser Doctor { get; set; }
    }
}