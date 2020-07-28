using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ML.Models;
using X.PagedList.Mvc;
using X.PagedList;
using Microsoft.AspNet.Identity;

namespace ML.Controllers
{
    public class processorController : Controller
    {
        private db_managment_labEntities4 db = new db_managment_labEntities4();

		[Authorize(Roles = "Admin")]
		public ActionResult Index(int? page, string d1, FormCollection fc)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var action = fc["submitButton"];
            var c = from m in db.processors
                    select m;
            c = c.OrderBy(ca => ca.Name);
            int pageSize = 18;
            int pageNumber = page ?? 1;

            switch (action)
            {
                case "Search":
                    if (!String.IsNullOrEmpty(d1))
                    {
                        c = c.Where(s => s.Name.Contains(d1));
                    }
                    return View(c.ToList().ToPagedList(pageNumber, pageSize));

                case "Reset":
                    ModelState.Clear();
                    return View(c.ToList().ToPagedList(pageNumber, pageSize));

                default:
                    return View(c.ToList().ToPagedList(pageNumber, pageSize));
            }
        }
		
        public ActionResult Details(Guid? id)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            processor processor = db.processors.Find(id);
            if (processor == null)
            {
                return HttpNotFound();
            }
            return View(processor);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GID,Name,Specs,Image,Manufacture")] processor processor)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                processor.GID = Guid.NewGuid();
                db.processors.Add(processor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(processor);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Edit(Guid? id)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            processor processor = db.processors.Find(id);
            if (processor == null)
            {
                return HttpNotFound();
            }
            return View(processor);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GID,Name,Specs,Image,Manufacture")] processor processor)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                db.Entry(processor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(processor);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Delete(Guid? id)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            processor processor = db.processors.Find(id);
            if (processor == null)
            {
                return HttpNotFound();
            }
            return View(processor);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            processor processor = db.processors.Find(id);
            db.processors.Remove(processor);
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
