using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CareTakerCT.Models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Clinic Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name= "Clinic Address")]
        public string Address{ get; set; }
    }
}