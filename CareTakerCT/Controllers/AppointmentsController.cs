using CareTakerCT.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CareTakerCT.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Appointments
        public ActionResult Index()
        {
            // User Id
            var UserId = User.Identity.GetUserId();

            // Initialization and also avoid unauthorized access
            var result = new List<Appointment>(); 

            if (User.IsInRole("doctor"))
            {
                result = db.Appointments.Where(a => a.DoctorId == UserId.ToString()).Include(a => a.Clinic).Include(a => a.Doctor).ToList();
            }
            else if (User.IsInRole("patient"))
            {
                result = db.Appointments.Where(a => a.PatientId == UserId.ToString()).Include(a => a.Clinic).Include(a => a.Doctor).ToList();
            }
            else if (User.IsInRole("admin"))
            {
                result = db.Appointments.Include(a => a.Clinic).Include(a => a.Doctor).ToList();
            }

            ViewBag.Appointment = result;
            return View(result);
        }

        public JsonResult GetAppointments(string id)
        {
            var appointments = new List<Appointment>();

            appointments = db.Appointments.Where(a => a.DoctorId == id.ToString()).ToList();

                
            return new JsonResult
            {
                Data = appointments,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        [Authorize]
        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var appointment = db.Appointments.Where(a=>a.Id ==id).Include(a => a.Doctor).Include(a => a.Clinic).FirstOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        [Authorize]
        // GET: Appointments/Create
        public ActionResult Create()
        {
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id)).ToList();

            ViewBag.DoctorId = new SelectList(doctors, "Id", "FirstName");
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name");
            return View();
        }
        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]  // We also use this annotation to prevent malicious script
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,BookTime,Description,ClinicId,DoctorId")] Appointment appointment)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(HttpUtility.HtmlEncode(appointment.Description));

            appointment.Description = sb.ToString();

            // Encode the HTML entities to ensure data is properly sanitized and protected against XSS attacks
            string strDes = HttpUtility.HtmlEncode(appointment.Description);
            appointment.Description = strDes;

            if (ModelState.IsValid)
            {
                // Query on the database for existing appointments
                var existingAppointments = db.Appointments
                    .Where(a => a.DoctorId == appointment.DoctorId)
                    .ToList();

                // Check if new appointment conflicts with existing appointment
                bool hasConflict = false;
                foreach (var existingAppointment in existingAppointments)
                {
                    DateTime existingStart = existingAppointment.BookTime;
                    DateTime existingEnd = existingStart.AddHours(1); // Assume each appointment is 1 hour long
                    DateTime newStart = appointment.BookTime;
                    DateTime newEnd = newStart.AddHours(1);

                    if ((newStart >= existingStart && newStart < existingEnd) ||
                        (newEnd > existingStart && newEnd <= existingEnd))
                    {
                        hasConflict = true;
                        break; // once a conflict occur, we stop checking
                    }
                }

                if (hasConflict)
                {

                    ViewBag.Result = "There is a time conflict between the new appointment and the existing appointment. Please choose another time.";
                } else
                {
                    // If there are no conflicts, save the new appointment to the database

                    // Here we add patientId to build the relationship between Patient and Appointment(Restrict the appointment is made from paitient)
                    appointment.PatientId = User.Identity.GetUserId();

                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id)).ToList();

            ViewBag.DoctorId = new SelectList(doctors, "Id", "FirstName");
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name", appointment.ClinicId);
            var doctor = appointment.Doctor;
            var bookTime = appointment.BookTime;


            return View(appointment);
        }


        [ValidateAntiForgeryToken]  // We also use this annotation to prevent malicious script
        [Authorize(Roles = "admin,doctor")]
        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id)).ToList();

            ViewBag.DoctorId = new SelectList(doctors, "Id", "FirstName");

            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name", appointment.ClinicId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,doctor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookTime,Description,ClinicId,DoctorId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id)).ToList();

            ViewBag.DoctorId = new SelectList(doctors, "Id", "FirstName");

            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name", appointment.ClinicId);
            return View(appointment);
        }

        [Authorize(Roles = "admin,doctor")]
        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [Authorize(Roles = "admin,doctor")]
        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}
