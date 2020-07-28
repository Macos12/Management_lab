using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
			Response.StatusCode = 404;
			return View();
        }
		public ActionResult Bad()
		{
			Response.StatusCode = 400;
			return View();
		}
	}
}