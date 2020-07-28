//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ML.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	public partial class PcProblem
	{
		public System.Guid GID { get; set; }
		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "Tell little more about the problem!")]
		public string MoreDetails { get; set; }
		public string username { get; set; }
		public Nullable<System.DateTime> date { get; set; }
		public Nullable<bool> IsSolved { get; set; }
		[Required(ErrorMessage = "Category field is required!")]
		public Nullable<int> cat_problem_id { get; set; }
		[Required(ErrorMessage = "Choose one of the Pc!")]
		public Nullable<System.Guid> NamePc { get; set; }
		[Required(ErrorMessage = "Choose one of the item that have problem!")]
		public Nullable<System.Guid> itemGID { get; set; }
		[Required(ErrorMessage = "Choose one of the lab room!")]
		public Nullable<int> lab_id { get; set; }
		

		public virtual CategoryProblem CategoryProblem { get; set; }
		public virtual Pclab Pclab { get; set; }
		public virtual lab lab { get; set; }
		public IEnumerable<SelectListItem> itemGIDList { get; set; }
	}
}