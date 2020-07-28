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
    public class hard_diskController : Controller
    {
        private db_managment_labEntities4 db = new db_managment_labEntities4();

		[Authorize(Roles = "Admin")]
		public ActionResult Index(int? page, string d1, FormCollection fc)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var action = fc["submitButton"];
            var c = from m in db.hard_disk
                    select m;
            c = c.OrderBy(ca => ca.CategoryHardDisk.cat_hard_disk_id).ThenBy(n => n.Name);
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
            hard_disk hard_disk = db.hard_disk.Find(id);
            if (hard_disk == null)
            {
                return HttpNotFound();
            }
            return View(hard_disk);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            ViewBag.cat_hard_disk_id = new SelectList(db.CategoryHardDisks, "cat_hard_disk_id", "cat_hard_disk_name");
            return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GID,Name,Specs,Image,Manufacture,cat_hard_disk_id")] hard_disk hard_disk)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                hard_disk.GID = Guid.NewGuid();
                db.hard_disk.Add(hard_disk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cat_hard_disk_id = new SelectList(db.CategoryHardDisks, "cat_hard_disk_id", "cat_hard_disk_name", hard_disk.cat_hard_disk_id);
            return View(hard_disk);
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
            hard_disk hard_disk = db.hard_disk.Find(id);
            if (hard_disk == null)
            {
                return HttpNotFound();
            }
            ViewBag.cat_hard_disk_id = new SelectList(db.CategoryHardDisks, "cat_hard_disk_id", "cat_hard_disk_name", hard_disk.cat_hard_disk_id);
            return View(hard_disk);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GID,Name,Specs,Image,Manufacture,cat_hard_disk_id")] hard_disk hard_disk)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                db.Entry(hard_disk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cat_hard_disk_id = new SelectList(db.CategoryHardDisks, "cat_hard_disk_id", "cat_hard_disk_name", hard_disk.cat_hard_disk_id);
            return View(hard_disk);
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
            hard_disk hard_disk = db.hard_disk.Find(id);
            if (hard_disk == null)
            {
                return HttpNotFound();
            }
            return View(hard_disk);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            hard_disk hard_disk = db.hard_disk.Find(id);
            db.hard_disk.Remove(hard_disk);
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
