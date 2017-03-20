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
    public class EnvMgmtController : Controller
    {
       [HttpGet]
        public ActionResult AddEnv()
        {
            Env environment = new Env();
            return View(environment);
        }

       

        public ActionResult AppVersions()
        {
            List<string> appVersions = new List<string>();
            AwsWV awsClient = new AwsWV();
            appVersions = awsClient.GetVersions("EventsAPI");

            return View();
        }

        public async Task<PartialViewResult> EnvDetails(string name)
        {
            Env env = new Env();
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", name.ToLower());
            env.name = name;
            env.aws_region = await myconsul.getPair(path + "aws_region");
            env.protectedEnv = Convert.ToBoolean(await myconsul.getPair(path + "protected"));
            env.vpc_cidr = await myconsul.getPair(path + "vpc_cidr");

            return PartialView("_EnvDetails", env);

        }

        public async Task<ActionResult> EnvDelete(string name)
        {
            ConsulWV myconsul = new ConsulWV();
            Jenkins jenkins = new Jenkins();
            string consulPath = String.Format("environments/{0}/", name.ToLower());
            var awsRegion = await myconsul.getPair(consulPath + "aws_region");

            List<Parameter> parameterList = new List<Parameter>();
            parameterList.Add(new Parameter { name = "ENVIRONMENT", value = name });
            parameterList.Add(new Parameter { name = "AWS_REGION", value = awsRegion });

            var result = await jenkins.ExecuteJob("Environment_Delete", parameterList);

            return Content("Success" + result);
        }

        public async Task<ActionResult> EnvBuild(string name)
        {
            ConsulWV myconsul = new ConsulWV();
            Jenkins jenkins = new Jenkins();
            string consulPath = String.Format("environments/{0}/", name.ToLower());
            var awsRegion = await myconsul.getPair(consulPath + "aws_region");

            List<Parameter> parameterList = new List<Parameter>();
            parameterList.Add(new Parameter { name = "ENVIRONMENT", value = name });
            parameterList.Add(new Parameter { name = "AWS_REGION", value = awsRegion });

            var result = await jenkins.ExecuteJob("Environment_Add", parameterList);
            
            return Content("Success" + result);

        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection formCollection)
        {
            Env env = new Env();
            TryUpdateModel(env);
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", env.name.ToLower());
            await myconsul.putFolder(path);
            await myconsul.putConsul(path + "aws_region", env.aws_region);
            await myconsul.putConsul(path + "vpc_cidr", env.vpc_cidr);
            await myconsul.putConsul(path + "protected", env.protectedEnv.ToString());
            await myconsul.putConsul(path + "updated", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
            return RedirectToAction("Index");
        }

        [HttpGet]
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