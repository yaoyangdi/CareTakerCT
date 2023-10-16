using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareTakerCT.Models
{
    public class DoctorAppointmentData
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime? Date { get; set; }
        public int AppointmentCount { get; set; }
    }
}