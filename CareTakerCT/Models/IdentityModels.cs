using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace CareTakerCT.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // New attribute added
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public ICollection<DoctorRatings> DoctorRatings { get; set; } 
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<SendEmail> SendEmails { get; set; }

        public DbSet<Files> Files { get; set; }

        public DbSet<DoctorRatings> DoctorRatings { get; set; }

    }
}