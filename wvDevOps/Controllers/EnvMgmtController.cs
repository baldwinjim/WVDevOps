using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wvDevOps.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace wvDevOps.Controllers
{
    public class EnvMgmtController : Controller
    {
        // GET: EnvMgmt
        public async Task<ActionResult> Index()
        {
            ConsulWV myconsul = new ConsulWV();

            //var result = await myconsul.putConsul("Dummy", "QA Environment");
            var result = await myconsul.getEnvironments("environments");

            ViewBag.ConsulResult = result;

            return View();
        }

        public ActionResult New()
        {
            return View();
        }
    }

}