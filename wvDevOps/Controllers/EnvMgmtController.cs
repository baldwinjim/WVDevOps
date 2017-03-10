using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wvDevOps.Helpers;
using System.Threading;
using System.Threading.Tasks;
using wvDevOps.Models;

namespace wvDevOps.Controllers
{
    public class EnvMgmtController : Controller
    {
       [HttpGet]
        public ActionResult AddEnv()
        {
            Env environment = new Env();
            return View(environment);
        }

        [HttpPost]
        public async Task<ActionResult> AddEnv(FormCollection formCollection)
        {
            //Env environment = new Env();
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", formCollection["name"].ToLower());
            var result = await myconsul.putFolder(path);
            result = await myconsul.putConsul(path + "vpc_cidr", formCollection["vpc_cidr"]);
            result = await myconsul.putConsul(path + "aws_region", formCollection["aws_region"]);
            result = await myconsul.putConsul(path + "updated", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));

            return RedirectToAction("Index");
            //return Content("Success");
        }


        public async Task<ActionResult> Index()
        {
            ConsulWV myconsul = new ConsulWV();

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