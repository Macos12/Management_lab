using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Models
{
	public class labpcViewModel
	{
		public List<PerDay> perDays { get; set; }
		public List<PerMonth> perMonth { get; set; }
		public List<PerPc> perpc { get; set; }
		public List<PerLabGeneral> perlabgeneral { get; set; }
		public List<PerLabUnsolved> perlabunsolved { get; set; }
		public List<PerLabSolved> perlabsolved { get; set; }
		public List<TotalProblemPerManufactureCategoryItem> totalproblempermanufacturecategoryitem { get; set; }

	}
}