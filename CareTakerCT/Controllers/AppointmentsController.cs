using CareTakerCT.Migrations;
using CareTakerCT.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
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

            var appointment = db.Appointments.Where(a => a.Id == id).Include(a => a.Doctor).Include(a => a.Clinic).FirstOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        [Authorize]
        [Route("Appointments/Create/")]
        // GET: Appointments/Create
        public ActionResult Create()
        {
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            var doctors = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id)).ToList();

            ViewBag.DoctorId = new SelectList(doctors, "Id", "FirstName");
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name");
            return View();
        }

        [Authorize]
        // GET: Appointments/Create
        [Route("Appointments/Create/{date}")]
        public ActionResult Create(string date)
        {
            // Convert the string to a DateTime object
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                // Format the date in "MM/DD/YYYY HH:mm" format
                string formattedDate = parsedDate.ToString("MM/dd/yyyy HH:mm");

                ViewBag.Date = formattedDate; // Pass the formatted date to the view
            }

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
                }
                else
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
            Appointment appointment = db.Appointments.Where(a => a.Id == id).Include(a => a.Doctor).Include(a => a.Clinic).FirstOrDefault();
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
        public ActionResult Chart()
        {

            // Define labels
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;
            var dateRange = Enumerable.Range(0, 1 + endDate.Subtract(oneMonthAgo).Days).Select(offset => DateTime.Parse(oneMonthAgo.AddDays(offset).ToString("yyyy-MM-dd")));
            var labels = dateRange.Select(date => date.ToString("yyyy-MM-dd")).ToList();


            // Define datasets
            // Retrieve data from Appointment Model
            var chartData = db.Appointments
                .Where(appointment => appointment.BookTime >= oneMonthAgo && appointment.BookTime <= endDate)
                .GroupBy(appointment => new
                {
                    DoctorId = appointment.DoctorId,
                    Date = DbFunctions.TruncateTime(appointment.BookTime)
                })
                .Select(group => new DoctorAppointmentData
                {
                    DoctorName = "",
                    DoctorId = group.Key.DoctorId,
                    Date = DbFunctions.TruncateTime(group.Key.Date),
                    AppointmentCount = group.Count(),
                })
                .ToList();


            // Update doctorName and Date in chartData
            var doctorIds = chartData.Select(data => data.DoctorId).Distinct().ToList();
            var doctorNames = db.Users.Where(user => doctorIds.Contains(user.Id))
                .ToDictionary(user => user.Id, user => user.FullName);

            foreach (var data in chartData)
            {

                data.DoctorName = doctorNames[data.DoctorId];


                System.Diagnostics.Debug.WriteLine("bbbbbbbbbbbbbbbbb" + "   "+ data.Date + "   " + data.DoctorName +"   " + data.AppointmentCount);

            }



            // Create a dictionary to store chart data grouped by Date and DoctorId
            var chartDataDict = chartData.GroupBy(data => new MyKey { Date = data.Date, DoctorId = data.DoctorId })
                .ToDictionary(group => group.Key, group => group.First());



            var datasets = new List<ChartDataset>(); // Our final return Model list

            foreach (var doctorId in doctorIds)  // For each doctor exist in the database, iterate to get the dataset required by the chart
            {
                var chartdataset = new ChartDataset();
                chartdataset.Label = doctorNames[doctorId].ToString();

                foreach (var date in dateRange)
                {
                    // Create a key to search for data in the dictionary
                    var key = new MyKey { Date = date, DoctorId = doctorId };

                    // If data exists for the key, add it to the mergedChartData
                    if (chartDataDict.ContainsKey(key))
                    {
                        System.Diagnostics.Debug.WriteLine("aaaaaaaaaaaaaaaaaa" + chartDataDict[key].DoctorName + "       " + chartDataDict[key].AppointmentCount);

                        chartdataset.Data.Add(chartDataDict[key].AppointmentCount);
                    }
                    // If data doesn't exist, create a new record with count = 0
                    else
                    {
                        chartdataset.Data.Add(0);
                    }
                }
                datasets.Add(chartdataset);
            }

            var chartModel = new ChartModel();
            chartModel.Labels = labels;
            chartModel.Datasets = datasets;

            return View(chartModel);
        }
    }
}

public class ChartDataset
{
    public ChartDataset()
    {
        Data = new List<int>();
    }
    public string Label { get; set; }
    public List<int> Data { get; set; }
}

public class ChartModel
{
    public List<string> Labels { get; set; }
    public List<ChartDataset> Datasets { get; set; }
}


public class MyKey
{
    public DateTime? Date { get; set; }
    public string DoctorId { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is MyKey other)
        {
            return Date == other.Date && DoctorId == other.DoctorId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Date.GetHashCode();
            hash = hash * 23 + DoctorId.GetHashCode();
            return hash;
        }
    }
}