using ML.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CascadeController : Controller
    {
		// GET: Cascade
		private db_managment_labEntities4 db = new db_managment_labEntities4();
		List<PerDay> perdays = new List<PerDay>();
		List<PerMonth> permonths = new List<PerMonth>();
		List<PerPc> perpcs = new List<PerPc>();
		List<PerLabGeneral> perlabsgeneral = new List<PerLabGeneral>();
		List<PerLabUnsolved> perlabsunsolved = new List<PerLabUnsolved>();
		List<PerLabSolved> perlabssolved = new List<PerLabSolved>();
		List<TotalProblemPerManufactureCategoryItem> TotalProblemPerManufactureCategoryItems = new List<TotalProblemPerManufactureCategoryItem>();

		public ActionResult Index()
        {
			//ana hmera
			var hmera = (from x in db.PcProblems
						group x by new { Column1 = SqlFunctions.DatePart("YEAR", x.date), Column2 = SqlFunctions.DatePart("MONTH", x.date), Column3 = SqlFunctions.DatePart("DAY", x.date) }
						  into g
						  orderby g.Key.Column1,g.Key.Column2,g.Key.Column3
						select new { year = g.Key.Column1, month = g.Key.Column2, day = g.Key.Column3,count=g.Count() }).ToList();
			
			///provlhmata ana mhna 
			var mhna = (from x in db.PcProblems
						   group x by new { Column1 = SqlFunctions.DatePart("MONTH", x.date), Column2 = SqlFunctions.DatePart("YEAR", x.date) } into g
						   select new { month = g.Key.Column1, year = g.Key.Column2, count = g.Count() }).ToList();
			
			foreach (var item in hmera)
			{
				perdays.Add(new PerDay
				{
					year = (int)item.year,
					month = (int)item.month,
					day = (int)item.day,
					count = (int)item.count
				});
			}
			foreach (var item in mhna)
			{
				permonths.Add(new PerMonth
				{
					year = (int)item.year,
					month = (int)item.month,
					count = (int)item.count
				});
			}
			
			//synolika provlimata poy exoyn paroustiastei ston kathe ypologisti
			var problimataanapc= (from kl in db.PcProblems
						  join k in db.Pclabs
						  on kl.NamePc equals k.GID
						  group new { kl,k } by new { Column1=kl.NamePc,Column2=k.NamePc } into g
						  select new {col=g.Key.Column1,name=g.Key.Column2,count=g.Count()}).ToList();
			foreach (var item in problimataanapc)
			{
				perpcs.Add(new PerPc
				{
					col = (Guid)item.col,
					name = item.name,
					count = (int)item.count
				});
			}
			//synolika provlimata ana ergasthrio genika
			var problembylabgeneral = (from k in db.PcProblems
										   join kl in db.labs
										   on k.lab_id equals kl.lab_id
										   group new { k, kl } by new { Column1 = k.lab_id, Column2 = kl.lab_name } into g
										   select new { lab_id = g.Key.Column1, lab_name = g.Key.Column2, count = g.Count() }).ToList();
			foreach (var item in problembylabgeneral)
			{
				perlabsgeneral.Add(new PerLabGeneral
				{
					lab_id=(int)item.lab_id,
					lab_name=item.lab_name,
					count=(int)item.count
				});
			}
			//synolika provlimata ana ergasthrio alyta
			var problembylabunsolved= (from k in db.PcProblems
									   join kl in db.labs 
									   on k.lab_id equals kl.lab_id
									   where k.IsSolved==false
									   group new { k, kl } by new { Column1 = k.lab_id, Column2 = kl.lab_name } into g
									   select new { lab_id = g.Key.Column1, lab_name = g.Key.Column2, count = g.Count() }).ToList();

			foreach (var item in problembylabunsolved)
			{
				perlabsunsolved.Add(new PerLabUnsolved
				{
					lab_id = (int)item.lab_id,
					lab_name = item.lab_name,
					count = (int)item.count
				});
			}
			//synolika provlimata ana ergasthrio lymena
			var problembylabsolved= (from k in db.PcProblems
									 join kl in db.labs
									 on k.lab_id equals kl.lab_id
									 where k.IsSolved == true
									 group new { k, kl } by new { Column1 = k.lab_id, Column2 = kl.lab_name } into g
									 select new { lab_id = g.Key.Column1, lab_name = g.Key.Column2, count = g.Count()}).ToList();
			foreach (var item in problembylabsolved)
			{
				perlabssolved.Add(new PerLabSolved
				{
					lab_id = (int)item.lab_id,
					lab_name = item.lab_name,
					count = (int)item.count
				});
			}
			//
			var totalproblemspermanufacurecooler = (from k in db.PcProblems
													join k1 in db.coolers
													on k.itemGID equals k1.GID
													group k1 by new { Column1 = k1.Manufacture } into g
													orderby g.Count() descending
													select new { Manufacture = g.Key.Column1, count = g.Count()}).Take(1).ToList();
			foreach (var item in totalproblemspermanufacurecooler)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture=item.Manufacture,
					count=(int)item.count,
					itemCategory="cooler"
				});
			}
			var totalproblemspermanufacuregpu = (from k in db.PcProblems
													join k1 in db.gpus
													on k.itemGID equals k1.GID
													group k1 by new { Column1 = k1.Manufacture } into g
													orderby g.Count() descending
													select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacuregpu)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "gpu"
				});
			}
			var totalproblemspermanufacureharddisk = (from k in db.PcProblems
												 join k1 in db.hard_disk
												 on k.itemGID equals k1.GID
												 group k1 by new { Column1 = k1.Manufacture } into g
												 orderby g.Count() descending
												 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacureharddisk)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "hard disk"
				});
			}
			var totalproblemspermanufacuremotherboard = (from k in db.PcProblems
													  join k1 in db.motherboards
													  on k.itemGID equals k1.GID
													  group k1 by new { Column1 = k1.Manufacture } into g
													  orderby g.Count() descending
													  select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacuremotherboard)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "motherboard"
				});
			}
			var totalproblemspermanufacureoptical = (from k in db.PcProblems
														 join k1 in db.opticals
														 on k.itemGID equals k1.GID
														 group k1 by new { Column1 = k1.Manufacture } into g
														 orderby g.Count() descending
														 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacureoptical)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "optical"
				});
			}
			var totalproblemspermanufacurecase = (from k in db.PcProblems
														 join k1 in db.pc_case
														 on k.itemGID equals k1.GID
														 group k1 by new { Column1 = k1.Manufacture } into g
														 orderby g.Count() descending
														 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacurecase)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "case"
				});
			}
			var totalproblemspermanufacureprocessor = (from k in db.PcProblems
														 join k1 in db.processors
														 on k.itemGID equals k1.GID
														 group k1 by new { Column1 = k1.Manufacture } into g
														 orderby g.Count() descending
														 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacureprocessor)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "processor"
				});
			}
			var totalproblemspermanufacurepsu = (from k in db.PcProblems
														 join k1 in db.psus
														 on k.itemGID equals k1.GID
														 group k1 by new { Column1 = k1.Manufacture } into g
														 orderby g.Count() descending
														 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacurepsu)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "motherboard"
				});
			}
			var totalproblemspermanufacureram = (from k in db.PcProblems
														 join k1 in db.rams
														 on k.itemGID equals k1.GID
														 group k1 by new { Column1 = k1.Manufacture } into g
														 orderby g.Count() descending
														 select new { Manufacture = g.Key.Column1, count = g.Count() }).Take(1).ToList();
			foreach (var item in totalproblemspermanufacureram)
			{
				TotalProblemPerManufactureCategoryItems.Add(new TotalProblemPerManufactureCategoryItem
				{
					Manufacture = item.Manufacture,
					count = (int)item.count,
					itemCategory = "motherboard"
				});
			}

			var c = new labpcViewModel
			{
				perDays = perdays,
				perMonth = permonths,
				perpc=perpcs,
				perlabgeneral=perlabsgeneral,
				perlabunsolved= perlabsunsolved,
				perlabsolved=perlabssolved,
				totalproblempermanufacturecategoryitem=TotalProblemPerManufactureCategoryItems
			};
			return View(c);
        }
		public JsonResult GetNamePcList(int lab_id)
		{
			var ddlCity = db.Pclabs.Where(x => x.lab_id == lab_id).OrderBy(k=>k.NamePc).ToList();
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
		public JsonResult GetNamePartList(Guid NamePc)
		{
			//var dcase = from m in db.Pclabs where m.GID == NamePc select m.caseID;
			var ddlCity = db.Pclabs.Where(x => x.GID == NamePc).ToList();
			List<SelectListItem> licities = new List<SelectListItem>();
			licities.Add(new SelectListItem { Text = "--Select part of pc--", Value = "0" });
			if (ddlCity != null)
			{
				foreach (var x in ddlCity)
				{
					licities.Add(new SelectListItem { Text = x.pc_case.Name, Value = x.caseID.ToString() });
					licities.Add(new SelectListItem { Text = x.motherboard.Name, Value = x.motherboardID.ToString() });
					licities.Add(new SelectListItem { Text = x.psu.Name, Value = x.psu.ToString() });
					licities.Add(new SelectListItem { Text = x.ram.Name, Value = x.ram.ToString() });
					licities.Add(new SelectListItem { Text = x.hard_disk.Name, Value = x.hard_diskID.ToString() });
					licities.Add(new SelectListItem { Text = x.gpu.Name, Value = x.gpuID.ToString() });
					if (x.opticalID != null)
					{
						licities.Add(new SelectListItem { Text = x.optical.Name, Value = x.opticalID.ToString() });
					}
					if (x.coolerID != null)
					{
						licities.Add(new SelectListItem { Text = x.cooler.Name, Value = x.coolerID.ToString() });
					}
				}
			}
			return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
		}

	}
}