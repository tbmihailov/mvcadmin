using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace MvcAdminResearch.Areas.MvcAdmin.Controllers
{
    public class PanelController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
