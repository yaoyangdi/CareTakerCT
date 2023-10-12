using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareTakerCT.Models
{
    public class ClinicDoctorEmailViewModels
    {
        public int Id { get; set; }
        public List<Clinic> Clinics { get; set; }
        public List<ApplicationUser> Doctors { get; set; }
        public SendEmail SendEmail { get; set; }
        public Files File { get; set; }

        public List<float> DoctorRatings { get; set; }
        


    }
}