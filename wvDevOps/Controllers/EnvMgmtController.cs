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

        [HttpPost]
        public async Task<ActionResult> AddEnv(FormCollection formCollection)
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
            Jenkins jenkins = new Jenkins();
            string consulPath = String.Format("environments/{0}/", name.ToLower());
            var awsRegion = await myconsul.getPair(consulPath + "aws_region");

            List<Parameter> parameterList = new List<Parameter>();
            parameterList.Add(new Parameter { name = "ENVIRONMENT", value = name });
            parameterList.Add(new Parameter { name = "AWS_REGION", value = awsRegion });

            var result = await jenkins.ExecuteJob("Environment_Add", parameterList);
            
            //ConsulWV myconsul = new ConsulWV();
            //string path = String.Format("environments/{0}/", name.ToLower());
            //var awsRegion = await myconsul.getPair(path + "aws_region");
            //string url = "http://jenkinsapi.wvholdings.com:8080/job/Environment_Add/build?Token=Test1234";
            //string apiToken = "Takach56";
            //string userName = "jim";
            //string result;

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            //httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.Method = "POST";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(userName + ":" + apiToken);

            //httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);

            //httpWebRequest.PreAuthenticate = true;

            //List<Parameter> parameterList = new List<Parameter>();
            //parameterList.Add(new Parameter { name = "ENVIRONMENT", value = name });
            //parameterList.Add(new Parameter { name = "AWS_REGION", value = awsRegion });

            //string json = new JavaScriptSerializer().Serialize(new { parameter = parameterList.ToArray() });
            //json = System.Web.HttpUtility.UrlEncode(json);

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    streamWriter.Write("json=" + json);
            //}
            //try
            //{
            //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //    {
            //        result = streamReader.ReadToEnd();
            //    }
            //}
            //catch (WebException excp)
            //{
            //    using (var streamReader = new StreamReader(excp.Response.GetResponseStream()))
            //    {
            //        result = streamReader.ReadToEnd();
            //    }
            //}
            return Content("Success" + result);

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