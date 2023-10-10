using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CareTakerCT.Models
{
    public class SendEmail
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the contents")]
        public string Contents { get; set; }

        public Files file { get; set; }
    }
}