using CareTakerCT.Models;
using CareTakerCT.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
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

            return View(new SendEmail());
        }

        [HttpPost]
        public ActionResult Contact(SendEmail model, HttpPostedFileBase postedFile)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {

                    //var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
                    //model.file.Path = myUniqueFileName;

                    //string serverPath = Server.MapPath("~/Uploads/");
                    //string fileExtension = Path.GetExtension(postedFile.FileName);
                    //string filePath = model.file.Path + fileExtension;
                    //model.file.Path = filePath;
                    //postedFile.SaveAs(serverPath + model.file.Path);
                    //// db.Images.Add(model.file);
                    //db.SaveChanges();


                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    db.SendEmails.Add(model);
                    db.SaveChanges();

                    // Retrieve user email based on userId and define as fromEmail
                    var userId = User.Identity.GetUserId();
                    var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
                    String fromEmail = user.Email;

                    EmailSender es = new EmailSender();
                    es.Send(fromEmail, toEmail, subject, contents);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmail());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

    }
}