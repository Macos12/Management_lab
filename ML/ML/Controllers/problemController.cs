using Microsoft.AspNet.Identity;
using ML.Extensions;
using ML.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML.Controllers
{
	public class problemController : Controller
	{
		private db_managment_labEntities4 db = new db_managment_labEntities4();
		// GET: problem
		public ActionResult Index()
		{
			var myrole = new Class1();
			var userID = User.Identity.GetUserId();
			ViewBag.currentrole = myrole.a(userID);
			var c = from m in db.PcProblems
					select m;
			c = c.OrderByDescending(ca => ca.date);

			return View(c.ToList());
		}
		[Authorize(Roles = "Admin, User")]
		public ActionResult Create()
		{
			var myrole = new Class1();
			var userID = User.Identity.GetUserId();
			ViewBag.currentrole = myrole.a(userID);
			//ViewBag.cat_problem_id = new SelectList(db.CategoryProblems, "cat_problem_id", "cat_problem_name");
			List<SelectListItem> cat = new List<SelectListItem>();
			cat.Add(new SelectListItem { Text = "--Select one of the Categories--", Value = "0" });
			var categoriesofproblems = db.CategoryProblems.ToList();
			foreach (var mm in categoriesofproblems)
			{
				cat.Add(new SelectListItem { Text = mm.cat_problem_name, Value = mm.cat_problem_id.ToString() });
				ViewBag.categorylist = cat;
			}
			//ViewBag.NamePc = new SelectList(db.Pclabs.OrderBy(n => n.NamePc), "GID", "NamePc");
			var labss = db.labs.ToList();
			List<SelectListItem> li = new List<SelectListItem>();
			li.Add(new SelectListItem { Text = "--Select lab room--", Value = "0" });

			foreach (var m in labss)
			{
				li.Add(new SelectListItem { Text = m.lab_name, Value = m.lab_id.ToString() });
				ViewBag.lablist = li;
			}
			return View();
		}
		public JsonResult GetNamePcList(int? lab_id)
		{
			var ddlCity = db.Pclabs.Where(x => x.lab_id == lab_id).OrderBy(k => k.NamePc).ToList();
			List<SelectListItem> licities = new List<SelectListItem>();

			licities.Add(new SelectListItem { Text = "--Select name of pc--", Value = "0" });
			if (ddlCity != null)
			{
				foreach (var x in ddlCity)
				{
					licities.Add(new SelectListItem { Text = x.NamePc, Value = x.GID.ToString() });
				}
			}
			return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
		}
		public JsonResult GetNamePartList(Guid? NamePc)
		{
			//db.Configuration.ProxyCreationEnabled = false;
			var ddl = db.Pclabs.Where(x => x.GID == NamePc).ToList();
			List<SelectListItem> licities1 = new List<SelectListItem>();
			licities1.Add(new SelectListItem { Text = "--Select part of pc--", Value = "0" });
			if (ddl != null)
			{
				foreach (var x in ddl)
				{
					licities1.Add(new SelectListItem { Text = x.pc_case.Name, Value = x.caseID.ToString() });
					licities1.Add(new SelectListItem { Text = x.motherboard.Name, Value = x.motherboardID.ToString() });
					licities1.Add(new SelectListItem { Text = x.psu.Name, Value = x.psu.ToString() });
					licities1.Add(new SelectListItem { Text = x.ram.Name, Value = x.ram.ToString() });
					licities1.Add(new SelectListItem { Text = x.hard_disk.Name, Value = x.hard_diskID.ToString() });
					licities1.Add(new SelectListItem { Text = x.gpu.Name, Value = x.gpuID.ToString() });
					if (x.opticalID != null)
					{
						licities1.Add(new SelectListItem { Text = x.optical.Name, Value = x.opticalID.ToString() });
					}
					if (x.coolerID != null)
					{
						licities1.Add(new SelectListItem { Text = x.cooler.Name, Value = x.coolerID.ToString() });
					}
				}
			}
			return Json(new SelectList(licities1, "Value", "Text", JsonRequestBehavior.AllowGet));
		}
		public JsonResult Getitem(Guid? itemGID)
		{
			List<SelectListItem> licities1 = new List<SelectListItem>();
			return Json(new SelectList(licities1, "Value", "Text", JsonRequestBehavior.AllowGet));
		}
		[Authorize(Roles = "Admin, User")]
		[HttpPost]
		public ActionResult Create(PcProblem PcProblem, FormCollection formCollection)//,Guid? itemGID)
																					  //public ActionResult Create(FormCollection formCollection)
		{
			var myrole = new Class1();
			var userID = User.Identity.GetUserId();
			ViewBag.currentrole = myrole.a(userID);
			var i111 = formCollection["d1"];
			var saving1 = new PcProblem
			{
				GID = Guid.NewGuid(),
				//MoreDetails = formCollection["d4"],
				MoreDetails = PcProblem.MoreDetails,
				username = User.Identity.Getusern(),
				date = System.DateTime.Now,
				IsSolved = false,
				//cat_problem_id = Int32.Parse(formCollection["d0"]),
				cat_problem_id = PcProblem.cat_problem_id,
				//lab_id = Int32.Parse(formCollection["d1"]),
				lab_id = PcProblem.lab_id,
				//NamePc = Guid.Parse(formCollection["d2"]),
				NamePc = PcProblem.NamePc,
				//itemGID = Guid.Parse(formCollection["d3"])
				itemGID = PcProblem.itemGID
				//itemGID=new Guid(i111)
			};
			if (ModelState.IsValid)
			{
				var saving = new PcProblem
				{
					GID = Guid.NewGuid(),
					//MoreDetails = formCollection["d4"],
					MoreDetails = PcProblem.MoreDetails,
					username = User.Identity.Getusern(),
					date = System.DateTime.Now,
					IsSolved = false,
					//cat_problem_id = Int32.Parse(formCollection["d0"]),
					cat_problem_id = PcProblem.cat_problem_id,
					//lab_id = Int32.Parse(formCollection["d1"]),
					lab_id = PcProblem.lab_id,
					//NamePc = Guid.Parse(formCollection["d2"]),
					NamePc = PcProblem.NamePc,
					//itemGID = Guid.Parse(formCollection["d3"])
					itemGID = PcProblem.itemGID
					//itemGID = new Guid(i111)
				};
				db.PcProblems.Add(saving);
				db.SaveChanges();
				TempData["Success"] = "Υour request has been successfully submitted. We thoroughly check it to resolve it!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["Op"] = "Fill all the textboxes in order to submit a problem";

				return View("Create");
			}
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Solved(Guid id)
		{
			var saving = new PcProblem
			{
				GID = id,
				MoreDetails = (from R in db.PcProblems where R.GID == id select R.MoreDetails).FirstOrDefault(),
				username = (from R in db.PcProblems where R.GID == id select R.username).FirstOrDefault(),
				date = (from R in db.PcProblems where R.GID == id select R.date).FirstOrDefault(),
				IsSolved = true,
				cat_problem_id = (from R in db.PcProblems where R.GID == id select R.cat_problem_id).FirstOrDefault(),
				NamePc = (from R in db.PcProblems where R.GID == id select R.NamePc).FirstOrDefault(),
				lab_id = (from R in db.PcProblems where R.GID == id select R.lab_id).FirstOrDefault(),
				itemGID = (from R in db.PcProblems where R.GID == id select R.itemGID).FirstOrDefault()
			};
			PcProblem PcProblem = db.PcProblems.Find(id);
			db.PcProblems.Remove(PcProblem);
			db.SaveChanges();
			db.PcProblems.Add(saving);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}