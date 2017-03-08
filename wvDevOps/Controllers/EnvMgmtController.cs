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
        public ActionResult Index()
        {
            ConsulWV myconsul = new ConsulWV();

            bool x = myconsul.putConsul("Dummy", "QA Environment").GetAwaiter().GetResult();

            return Content(x.ToString());
            //return View();
        }
    }
}