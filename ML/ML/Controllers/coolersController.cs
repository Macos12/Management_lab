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
    public class coolersController : Controller
    {
        private db_managment_labEntities4 db = new db_managment_labEntities4();

		// GET: coolers
		[Authorize(Roles = "Admin")]
		public ActionResult Index(int? page, string d1, FormCollection fc)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var action = fc["submitButton"];
            var c = from m in db.coolers
                          select m;
            c = c.OrderBy(ca => ca.CategoryCooler.cat_cooler_id).ThenBy(n => n.Name);
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
            cooler cooler = db.coolers.Find(id);
            if (cooler == null)
            {
                return HttpNotFound();
            }
            return View(cooler);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            ViewBag.cat_cooler_id = new SelectList(db.CategoryCoolers, "cat_cooler_id", "cat_cooler_name");
            return View();
        }
		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GID,Name,Specs,Image,Manufacture,cat_cooler_id")] cooler cooler)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                cooler.GID = Guid.NewGuid();
                db.coolers.Add(cooler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cat_cooler_id = new SelectList(db.CategoryCoolers, "cat_cooler_id", "cat_cooler_name", cooler.cat_cooler_id);
            return View(cooler);
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
            cooler cooler = db.coolers.Find(id);
            if (cooler == null)
            {
                return HttpNotFound();
            }
            ViewBag.cat_cooler_id = new SelectList(db.CategoryCoolers, "cat_cooler_id", "cat_cooler_name", cooler.cat_cooler_id);
            return View(cooler);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GID,Name,Specs,Image,Manufacture,cat_cooler_id")] cooler cooler)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                db.Entry(cooler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cat_cooler_id = new SelectList(db.CategoryCoolers, "cat_cooler_id", "cat_cooler_name", cooler.cat_cooler_id);
            return View(cooler);
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
            cooler cooler = db.coolers.Find(id);
            if (cooler == null)
            {
                return HttpNotFound();
            }
            return View(cooler);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            cooler cooler = db.coolers.Find(id);
            db.coolers.Remove(cooler);
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
