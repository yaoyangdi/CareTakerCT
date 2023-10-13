using CareTakerCT.Migrations;
using CareTakerCT.Models;
using CareTakerCT.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CareTakerCT.Controllers
{


    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            List<Clinic> clinics = db.Clinics.ToList();
            var doctorRatings = new List<float>();
            var doctors = new List<ApplicationUser>();

            foreach (Clinic clinic in clinics)
            {
                var d = db.Users.Where(u => u.Id == clinic.DoctorId).FirstOrDefault();
                doctors.Add(d);
            }

            foreach (ApplicationUser doc in doctors)
            {
                var d = db.DoctorRatings.Where(dr => dr.DoctorId == doc.Id)
                            .Select(dr => dr.Value) // Select the rating values
                            .DefaultIfEmpty(0)     // Handle empty collection
                            .Average();            // Calculate the average

                doctorRatings.Add((float)d);
            }



            var viewModel = new Models.ClinicDoctorEmailViewModels
            {
                Doctors = doctors,
                Clinics = db.Clinics.ToList(),
                SendEmail = new SendEmail(),
                DoctorRatings = doctorRatings,
            };

            return View(viewModel);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Contact(Models.ClinicDoctorEmailViewModels viewModel, HttpPostedFileBase postedFile, string[] selectedDoctors, int? SelectedClinicId)
        {
            // Initialize the viewmodel when make a post request
            List<Clinic> clinics = db.Clinics.ToList();
            var doctors = new List<ApplicationUser>();
            var doctorRatings = new List<float>();

            foreach (Clinic clinic in clinics)
            {
                var d = db.Users.Where(u => u.Id == clinic.DoctorId).FirstOrDefault();
                doctors.Add(d);
            }

            foreach (ApplicationUser doc in doctors)
            {
                var d = db.DoctorRatings.Where(dr => dr.DoctorId == doc.Id)
                            .Select(dr => dr.Value) // Select the rating values
                            .DefaultIfEmpty(0)     // Handle empty collection
                            .Average();            // Calculate the average

                doctorRatings.Add((float)d);
            }

            viewModel.Clinics = clinics;
            viewModel.Doctors = doctors;
            viewModel.DoctorRatings = doctorRatings;

            try
            {

                // Send emails to selected doctors
                if (selectedDoctors != null && selectedDoctors.Length > 0)
                {
                    foreach (var doctorId in selectedDoctors)
                    {
                        var doctor = db.Users.Where(u => u.Id == doctorId).FirstOrDefault();
                        if (doctor != null)
                        {
                            // Email sending logic

                            // Handle file upload
                            Files file = null;
                            if (postedFile != null)
                            {
                                string fileName = Path.GetFileName(postedFile.FileName);

                                // Save the file to the Uploads directory
                                string serverPath = Server.MapPath("~/Uploads/");
                                string filePath = serverPath + fileName;
                                // check if file not exist
                                if (!System.IO.File.Exists(filePath))
                                {
                                    // save file into folder directory
                                    postedFile.SaveAs(filePath);
                                }
                                else
                                {
                                    // delete file from folder
                                    System.IO.File.Delete(filePath);

                                    // save file into folder directory
                                    postedFile.SaveAs(filePath);
                                }


                                // Set the file information in the SendEmail model
                                file = new Files
                                {
                                    Path = filePath, // Save the file path or any relevant information
                                    Name = fileName,
                                };
                                db.Files.Add(file);
                                db.SaveChanges();

                                viewModel.SendEmail.file = file;
                            }

                            // Handle plain text and other related email components
                            String toEmail = doctor.Email;
                            String subject = viewModel.SendEmail.Subject;
                            String contents = viewModel.SendEmail.Contents;

                            // Save to database
                            viewModel.SendEmail.ToEmail = doctor.Email;

                            db.SendEmails.Add(viewModel.SendEmail);
                            db.SaveChanges();

                            // Retrieve user email based on userId as from Email
                            if (User.Identity.GetUserId() != null)
                            {
                                var userId = User.Identity.GetUserId();
                                var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
                                String fromEmail = user.Email;

                                // Send the email
                                EmailSender es = new EmailSender();

                                HostingEnvironment.QueueBackgroundWorkItem(cy => es.Send(fromEmail, toEmail, subject, contents, file));

                                ViewBag.Pass = true;
                                ViewBag.Result = "Emails have been sent successfully";

                                ModelState.Clear();
                                viewModel.SendEmail = new SendEmail();

                            }
                            else // back end validation (In case Front end validation is violated)
                            {
                                ViewBag.Result = "You need to log in first!";
                            }
                        }
                    }
                    ;
                }
                else
                {
                    ViewBag.Pass = false;
                    ViewBag.Result = "Please select at least one doctor to send email!";
                }
            }
            catch
            {
                ViewBag.Pass = false;
                ViewBag.Result = "An error occurred while sending emails or processing the file upload";

            }

            return View(viewModel);
        }


        public ActionResult Calendar(string id)
        {
            ViewBag.DoctorId = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser doctor = db.Users.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);

        }
    }


}
