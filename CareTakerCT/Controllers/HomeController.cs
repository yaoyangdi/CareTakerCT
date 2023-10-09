using CareTakerCT.Models;
using CareTakerCT.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
        public ActionResult Contact(SendEmail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

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