using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace wvDevOps.Helpers
{
    public class Jenkins
    {
        private string jenkinsUrl { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private string apiToken { get; set; }

        public Jenkins()
        {
            jenkinsUrl = WebConfigurationManager.AppSettings["Jenkins"];
            username = "jenkins";
            password = "DRXDsggrCo57yF95Rzu3";
            apiToken = "g4y92&b5NHx1";
        }
        public async Task<string> ExecuteJob(string jobName, List<Parameter> parameters)
        {
            string env = null;
            ConsulWV myconsul = new ConsulWV();
            string jenkinsPath = String.Format("job/{0}/build?Token={1}/", jobName, apiToken);
            //for (int i; i < parameters.Count(); i++)
            //{
            //    if (parameters[i].name == "ENVIRONMENT")
            //    {
            //        env = parameters[i].value;
            //        break;
            //    }
            //}

            //string consulPath = String.Format("environments/{0}/", env);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(jenkinsUrl);
            string result;
           

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";

            byte[] credentialBuffer = new UTF8Encoding().GetBytes(username + ":" + apiToken);

            httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
            httpWebRequest.PreAuthenticate = true;

            string json = new JavaScriptSerializer().Serialize(new { parameter = parameters.ToArray() });
            json = System.Web.HttpUtility.UrlEncode(json);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write("json=" + json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException excp)
            {
                using (var streamReader = new StreamReader(excp.Response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            return result;
        }
    }
}