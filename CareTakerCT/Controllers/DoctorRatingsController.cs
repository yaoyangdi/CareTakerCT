﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareTakerCT.Models;

namespace CareTakerCT.Controllers
{
    public class DoctorRatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DoctorRatings
        public ActionResult Index()
        {
            var doctorRatings = db.DoctorRatings.Include(d => d.Doctor);
            return View(doctorRatings.ToList());
        }

        // GET: DoctorRatings/Create
        public ActionResult Create()
        {
            var userRole = db.Roles.Where(r => r.Name == "doctor").FirstOrDefault();
            // Get all avaialble doctors in clinics

            List<Clinic> c = db.Clinics.ToList();
            List<Clinic> clinics = db.Clinics.ToList();
            var doctors = new List<ApplicationUser>();

            foreach (Clinic clinic in c)
            {
                var d = db.Users.Where(u => u.Id == clinic.DoctorId).FirstOrDefault();
                if (d != null)
                {
                    doctors.Add(d);
                }
                else
                {
                    clinics.Remove(clinic);
                }
            }
            ViewBag.DoctorId = new SelectList(doctors, "Id", "FullName");
            return View();
        }

        // POST: DoctorRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value,Comment,DoctorId")] DoctorRatings doctorRatings)
        {
            if (ModelState.IsValid)
            {
                db.DoctorRatings.Add(doctorRatings);
                db.SaveChanges();
                ApplicationUser doctor = db.Users.Where(u => u.Id == doctorRatings.DoctorId).FirstOrDefault();
                doctor.DoctorRatings.Add(doctorRatings);
                db.Users.AddOrUpdate(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Get all avaialble doctors in clinics

            List<Clinic> c = db.Clinics.ToList();
            List<Clinic> clinics = db.Clinics.ToList();
            var doctors = new List<ApplicationUser>();

            foreach (Clinic clinic in c)
            {
                var d = db.Users.Where(u => u.Id == clinic.DoctorId).FirstOrDefault();
                if (d != null)
                {
                    doctors.Add(d);
                }
                else
                {
                    clinics.Remove(clinic);
                }
            }
            ViewBag.DoctorId = new SelectList(doctors, "Id", "Email", doctorRatings.DoctorId);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        // GET: DoctorRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorRatings doctorRatings = db.DoctorRatings.Find(id);
            if (doctorRatings == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Users, "Id", "Email", doctorRatings.DoctorId);
            return View(doctorRatings);
        }

        [Authorize(Roles = "admin")]
        // POST: DoctorRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value,Comment,DoctorId")] DoctorRatings doctorRatings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorRatings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Users, "Id", "Email", doctorRatings.DoctorId);
            return View(doctorRatings);
        }


        [Authorize(Roles = "admin")]
        // GET: DoctorRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorRatings doctorRatings = db.DoctorRatings.Find(id);
            if (doctorRatings == null)
            {
                return HttpNotFound();
            }
            return View(doctorRatings);
        }

        [Authorize(Roles ="admin")]
        // POST: DoctorRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorRatings doctorRatings = db.DoctorRatings.Find(id);
            db.DoctorRatings.Remove(doctorRatings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
