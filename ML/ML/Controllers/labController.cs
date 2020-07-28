using ML.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ML.Controllers
{
    public class labController : Controller
    {

        private db_managment_labEntities4 db = new db_managment_labEntities4();
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: lab
        public ActionResult Index()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
			var c = from m in db.Pclabs
					select m;
			c = c.OrderBy(n=>n.lab.lab_name).ThenBy(ca => ca.NamePc);
			return View(c.ToList());
        }
		[Authorize(Roles ="Admin")]
        public ActionResult In()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var users = from u in context.Users
                        from ur in u.Roles
                        join r in context.Roles on ur.RoleId equals r.Id
                        select new
                        {
                            UserId = u.Id,
                            Name = u.UserName,
                            Email = u.Email,
                            Role = r.Name
                        };
            int counterAdmin=0, counterUser=0, counterPending=0, counterBanned=0;
            foreach (var item in users)
            {
                if (item.Role == "Admin")
                    counterAdmin = counterAdmin + 1;
                else if (item.Role == "User")
                    counterUser = counterUser + 1;
                else if (item.Role == "Pending")
                    counterPending = counterPending + 1;
                else if (item.Role == "Banned")
                    counterBanned = counterBanned + 1;
                    
            }
            ViewBag.counterAdmin = counterAdmin;
            ViewBag.counterUser = counterUser;
            ViewBag.counterPending = counterPending;
            ViewBag.counterBanned = counterBanned;
            //dashboard mod1 = new dashboard();
            //var ann = from m in db.announcements select m;
            //ann = ann.OrderByDescending(ca => ca.date).Take(4);
            //var pcprob= from m in db.PcProblems select m;
            //pcprob = pcprob.OrderByDescending(ca => ca.date).Take(4);
            var cm = new dashboard
            {
                announcements = db.announcements.OrderByDescending(ca => ca.date).Take(4).ToList(),
                PcProblems = db.PcProblems.OrderByDescending(ca => ca.date).Take(4).ToList()
            };
            //mod1.announcements = ann.ToList();
            //mod1.PcProblems = pcprob.ToList();
            
            return View(cm);
        }
		[HttpPost]
		public JsonResult IsPcNameExists(string NamePc, string temp)
        {
			if (NamePc == temp)
			{
				return Json(false, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var result = !db.Pclabs.Any(x => x.NamePc == NamePc);
				return Json(result, JsonRequestBehavior.AllowGet);
			}
        }
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
            var model = new Pclab
            {
				temp="",
                coolers = db.coolers.OrderBy(ca => ca.CategoryCooler.cat_cooler_id).ThenBy(n => n.Name).ToList(),
                gpus = db.gpus.OrderBy(ca => ca.Name).ToList(),
                hard_disks = db.hard_disk.OrderBy(ca => ca.CategoryHardDisk.cat_hard_disk_id).ThenBy(n => n.Name).ToList(),
                motherboards = db.motherboards.OrderBy(ca => ca.Name).ToList(),
                opticals = db.opticals.OrderBy(ca => ca.Name).ToList(),
                pc_cases = db.pc_case.OrderBy(ca => ca.Name).ToList(),
                processors = db.processors.OrderBy(ca => ca.Name).ToList(),
                psus = db.psus.OrderBy(ca => ca.Name).ToList(),
                rams = db.rams.OrderBy(ca => ca.Name).ToList()
            };
			ViewBag.lab_id = new SelectList(db.labs, "lab_id", "lab_name");
			return View(model);
        }
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(Pclab Pclab)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
			bool result1 = db.Pclabs.Any(x => x.NamePc == Pclab.NamePc);
			
			if (result1 == true)
			{
				ModelState.AddModelError("NamePc", "The given name Pc is in use!!!!");
			}
			if (ModelState.IsValid)
            {
                var saving = new Pclab
                {
                    GID = Guid.NewGuid(),
                    NamePc = Pclab.NamePc,
                    motherboardID = Pclab.motherboardID,
                    ramID = Pclab.ramID,
                    cpuID = Pclab.cpuID,
                    hard_diskID = Pclab.hard_diskID,
                    psuID = Pclab.psuID,
                    gpuID = Pclab.gpuID,
                    caseID = Pclab.caseID,
                    opticalID = Pclab.opticalID,
                    coolerID = Pclab.coolerID,
					lab_id=Pclab.lab_id
				};
                db.Pclabs.Add(saving);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            else
            {
                
                var model = new Pclab
                {
                    GID = Guid.NewGuid(),
                    NamePc = Pclab.NamePc,
                    motherboardID = Pclab.motherboardID,
                    mother = Pclab.mother,
                    ramID = Pclab.ramID,
                    rm = Pclab.rm,
                    cpuID = Pclab.cpuID,
                    cp = Pclab.cp,
                    hard_diskID = Pclab.hard_diskID,
                    hdd = Pclab.hdd,
                    psuID = Pclab.psuID,
                    ps = Pclab.ps,
                    gpuID = Pclab.gpuID,
                    gp = Pclab.gp,
                    caseID = Pclab.caseID,
                    cs = Pclab.cs,
                    opticalID = Pclab.opticalID,
                    opt = Pclab.opt,
                    coolerID = Pclab.coolerID,
                    cool = Pclab.cool,
                    coolers = db.coolers.OrderBy(ca => ca.CategoryCooler.cat_cooler_id).ThenBy(n => n.Name).ToList(),
                    gpus = db.gpus.OrderBy(ca => ca.Name).ToList(),
                    hard_disks = db.hard_disk.OrderBy(ca => ca.CategoryHardDisk.cat_hard_disk_id).ThenBy(n => n.Name).ToList(),
                    motherboards = db.motherboards.OrderBy(ca => ca.Name).ToList(),
                    opticals = db.opticals.OrderBy(ca => ca.Name).ToList(),
                    pc_cases = db.pc_case.OrderBy(ca => ca.Name).ToList(),
                    processors = db.processors.OrderBy(ca => ca.Name).ToList(),
                    psus = db.psus.OrderBy(ca => ca.Name).ToList(),
                    rams = db.rams.OrderBy(ca => ca.Name).ToList()
                };
				ViewBag.lab_id = new SelectList(db.labs, "lab_id", "lab_name", Pclab.lab_id);
				return View(model);
            }
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
			Guid? csid = (from R in db.Pclabs join t in db.pc_case on R.caseID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var csname = (from R in db.Pclabs where R.GID == id select R.pc_case.Name).FirstOrDefault();
			if (csid == default(Guid)&&string.IsNullOrEmpty(csname))
			{
				csid = null;
			}
			Guid? opid = (from R in db.Pclabs join t in db.opticals on R.opticalID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var opname = (from R in db.Pclabs where R.GID == id select R.optical.Name).FirstOrDefault();
			if (opid == default(Guid) && string.IsNullOrEmpty(opname))
			{
				opid = null;
			}
			Guid? coolid = (from R in db.Pclabs join t in db.coolers on R.coolerID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var coolname = (from R in db.Pclabs where R.GID == id select R.cooler.Name).FirstOrDefault();
			if (coolid == default(Guid) && string.IsNullOrEmpty(coolname))
			{
				coolid = null;
			}
			Guid? gpid = (from R in db.Pclabs join t in db.gpus on R.gpuID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var gpname = (from R in db.Pclabs where R.GID == id select R.gpu.Name).FirstOrDefault();
			if (gpid == default(Guid) && string.IsNullOrEmpty(gpname))
			{
				gpid = null;
			}
			Guid? hddid = (from R in db.Pclabs join t in db.hard_disk on R.hard_diskID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var hddname = (from R in db.Pclabs where R.GID == id select R.hard_disk.Name).FirstOrDefault();
			if (hddid == default(Guid) && string.IsNullOrEmpty(hddname))
			{
				hddid = null;
			}
			Guid? motherid = (from R in db.Pclabs join t in db.motherboards on R.motherboardID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var mothername = (from R in db.Pclabs where R.GID == id select R.motherboard.Name).FirstOrDefault();
			if (motherid == default(Guid) && string.IsNullOrEmpty(mothername))
			{
				motherid = null;
			}
			Guid? cpid = (from R in db.Pclabs join t in db.processors on R.cpuID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var cpname = (from R in db.Pclabs where R.GID == id select R.processor.Name).FirstOrDefault();
			if (cpid == default(Guid) && string.IsNullOrEmpty(cpname))
			{
				cpid = null;
			}
			Guid? psuid = (from R in db.Pclabs join t in db.psus on R.psuID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var psuname = (from R in db.Pclabs where R.GID == id select R.psu.Name).FirstOrDefault();
			if (psuid == default(Guid) && string.IsNullOrEmpty(psuname))
			{
				psuid = null;
			}
			Guid? ramid = (from R in db.Pclabs join t in db.rams on R.ramID equals t.GID where R.GID == id select t.GID).FirstOrDefault();
			var ramname = (from R in db.Pclabs where R.GID == id select R.ram.Name).FirstOrDefault();
			if (ramid == default(Guid) && string.IsNullOrEmpty(ramname))
			{
				ramid = null;
			}

			var Pclab = new Pclab {
                coolers = db.coolers.OrderBy(ca => ca.CategoryCooler.cat_cooler_id).ThenBy(n => n.Name).ToList(),
                gpus = db.gpus.OrderBy(ca => ca.Name).ToList(),
                hard_disks = db.hard_disk.OrderBy(ca => ca.CategoryHardDisk.cat_hard_disk_id).ThenBy(n => n.Name).ToList(),
                motherboards = db.motherboards.OrderBy(ca => ca.Name).ToList(),
                opticals = db.opticals.OrderBy(ca => ca.Name).ToList(),
                pc_cases = db.pc_case.OrderBy(ca => ca.Name).ToList(),
                processors = db.processors.OrderBy(ca => ca.Name).ToList(),
                psus = db.psus.OrderBy(ca => ca.Name).ToList(),
                rams = db.rams.OrderBy(ca => ca.Name).ToList(),
                GID=(Guid) id,
                NamePc = (from R in db.Pclabs where R.GID == id select R.NamePc).FirstOrDefault(),
                temp= (from R in db.Pclabs where R.GID == id select R.NamePc).FirstOrDefault(),

                coolerID = coolid,
                cool= coolname,
                gpuID = gpid,
                gp= gpname,
                hard_diskID = hddid,
                hdd= hddname,
                motherboardID = motherid,
                mother= mothername,
                opticalID = opid,
                opt= opname,
				caseID= csid,
				//caseID = (from R in db.Pclabs join t in db.pc_case on R.caseID equals t.GID where R.GID == id select t.GID).FirstOrDefault(),
				cs = csname,
                cpuID = cpid,
                cp= cpname,
                psuID = psuid,
                ps= psuname,
                ramID = ramid,
                rm= ramname
			};
			Pclab cooler = db.Pclabs.Find(id);

			ViewBag.lab_id = new SelectList(db.labs, "lab_id", "lab_name", cooler.lab_id);
			return View(Pclab);
        }
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(Pclab Pclab)
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole = myrole.a(userID);
			bool result1;
			
			//var checkpcname=string.Empty;
			if (Pclab.NamePc == Pclab.temp)
			{
				result1 = false;
			}
			else
			{
				var checkpcname = (db.Pclabs.Where(x => x.NamePc == Pclab.NamePc).FirstOrDefault());
				result1 = (checkpcname!=null) ? true : false;


			}
			if (result1 == true)
			{
				ModelState.AddModelError("NamePc", "The given name Pc is in use!!!!");
			}
			if (ModelState.IsValid)
			{
				var saving = new Pclab
				{
					GID = Pclab.GID,
					NamePc = Pclab.NamePc,
					motherboardID = Pclab.motherboardID,
					ramID = Pclab.ramID,
					cpuID = Pclab.cpuID,
					hard_diskID = Pclab.hard_diskID,
					psuID = Pclab.psuID,
					gpuID = Pclab.gpuID,
					caseID = Pclab.caseID,
					opticalID = Pclab.opticalID,
					coolerID = Pclab.coolerID,
					lab_id=Pclab.lab_id
				};
				db.Entry(saving).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				var model = new Pclab
				{
					coolers = db.coolers.OrderBy(ca => ca.CategoryCooler.cat_cooler_id).ThenBy(n => n.Name).ToList(),
					gpus = db.gpus.OrderBy(ca => ca.Name).ToList(),
					hard_disks = db.hard_disk.OrderBy(ca => ca.CategoryHardDisk.cat_hard_disk_id).ThenBy(n => n.Name).ToList(),
					motherboards = db.motherboards.OrderBy(ca => ca.Name).ToList(),
					opticals = db.opticals.OrderBy(ca => ca.Name).ToList(),
					pc_cases = db.pc_case.OrderBy(ca => ca.Name).ToList(),
					processors = db.processors.OrderBy(ca => ca.Name).ToList(),
					psus = db.psus.OrderBy(ca => ca.Name).ToList(),
					rams = db.rams.OrderBy(ca => ca.Name).ToList(),
					GID = Pclab.GID,
					NamePc = Pclab.NamePc,
					motherboardID = Pclab.motherboardID,
					mother = Pclab.mother,
					ramID = Pclab.ramID,
					rm = Pclab.rm,
					cpuID = Pclab.cpuID,
					cp = Pclab.cp,
					hard_diskID = Pclab.hard_diskID,
					hdd = Pclab.hdd,
					psuID = Pclab.psuID,
					ps = Pclab.ps,
					gpuID = Pclab.gpuID,
					gp = Pclab.gp,
					caseID = Pclab.caseID,
					cs = Pclab.cs,
					opticalID = Pclab.opticalID,
					opt = Pclab.opt,
					coolerID = Pclab.coolerID,
					cool = Pclab.cool
				};
				ViewBag.lab_id = new SelectList(db.labs, "lab_id", "lab_name", Pclab.lab_id);
				return View(model);
			}

			
        }
		[Authorize(Roles = "Admin")]
		public ActionResult DeletePc(Guid GID)
        {
            Pclab Pclab = db.Pclabs.Find(GID);
            db.Pclabs.Remove(Pclab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}