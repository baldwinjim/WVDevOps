using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wvDevOps.Helpers;
using System.Threading;
using System.Threading.Tasks;
using wvDevOps.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Jenkins.Core;


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
            Env env = new Env();
            TryUpdateModel(env);
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", env.name.ToLower());
            var result = await myconsul.putFolder(path);
            result = await myconsul.putConsul(path + "aws_region", env.aws_region);
            result = await myconsul.putConsul(path + "vpc_cidr", env.vpc_cidr);
            result = await myconsul.putConsul(path + "protected", env.protectedEnv.ToString());
            result = await myconsul.putConsul(path + "updated", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EnvDetails(string name)
        {
            Env env = new Env();
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", name.ToLower());
            env.name = name;
            env.aws_region = await myconsul.getPair(path + "aws_region");
            env.protectedEnv = Convert.ToBoolean(await myconsul.getPair(path + "protected"));
            env.vpc_cidr = await myconsul.getPair(path + "vpc_cidr");

            return View(env);

        }

        public async Task<ActionResult> EnvBuild(string name)
        {
            ConsulWV myconsul = new ConsulWV();
            string path = String.Format("environments/{0}/", name.ToLower());
            var awsRegion = await myconsul.getPair(path + "aws_region");
            var credentials = Encoding.ASCII.GetBytes("jim:Takach56");

            var job = new
            {
                ENVIRONMENT = name,
                AWS_REGION = awsRegion
            };

            HttpClient client = new HttpClient();
            
            client.BaseAddress = new Uri("http://jenkinsapi.wvholdings.com:8080");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(job.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("job/Environment_Add/buildWithParameters?Token=Test1234", content);

            return Content("Success" + response);

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