using System;
using System.Collections.Generic;
using System.Web.Mvc;
using wvDevOps.Helpers;
using System.Threading.Tasks;
using wvDevOps.Models;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

namespace wvDevOps.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application

        public async Task<ActionResult> Index()
        {
            ConsulWV myconsul = new Helpers.ConsulWV();

            var result = await myconsul.getEnvironments("environments");

            ViewBag.ConsulResult = result;

            return View();
        }

    }

    public async Task<PartialViewResult> EnvDetailsRO(string name)
    {
        Env env = new Env();
        ConsulWV myconsul = new ConsulWV();
        string path = String.Format("environments/{0}/", name.ToLower());
        env.name = name;
        env.aws_region = await myconsul.getPair(path + "aws_region");
        env.protectedEnv = Convert.ToBoolean(await myconsul.getPair(path + "protected"));
        env.vpc_cidr = await myconsul.getPair(path + "vpc_cidr");

        return PartialView("_EnvDetailsRO", env);

    }
}