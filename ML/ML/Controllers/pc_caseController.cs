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
    public class pc_caseController : Controller
    {
        private db_managment_labEntities4 db = new db_managment_labEntities4();

		[Authorize(Roles = "Admin")]
		public ActionResult Index(int? page, string d1, FormCollection fc)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var action = fc["submitButton"];
            var c = from m in db.pc_case
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
            pc_case pc_case = db.pc_case.Find(id);
            if (pc_case == null)
            {
                return HttpNotFound();
            }
            return View(pc_case);
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
        public ActionResult Create([Bind(Include = "GID,Name,Specs,Image,Manufacture")] pc_case pc_case)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                pc_case.GID = Guid.NewGuid();
                db.pc_case.Add(pc_case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pc_case);
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
            pc_case pc_case = db.pc_case.Find(id);
            if (pc_case == null)
            {
                return HttpNotFound();
            }
            return View(pc_case);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GID,Name,Specs,Image,Manufacture")] pc_case pc_case)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            if (ModelState.IsValid)
            {
                db.Entry(pc_case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pc_case);
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
            pc_case pc_case = db.pc_case.Find(id);
            if (pc_case == null)
            {
                return HttpNotFound();
            }
            return View(pc_case);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            pc_case pc_case = db.pc_case.Find(id);
            db.pc_case.Remove(pc_case);
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
