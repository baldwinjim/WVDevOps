using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Consul;
using System.Threading.Tasks;
using System.Text;

namespace wvDevOps.Helpers
{
    public class ConsulWV
    {
        public string consulUrl { get; set; }
        private static ConsulClient consulClient;

        public ConsulWV()
        {
            Uri consulUrl = new System.Uri(WebConfigurationManager.AppSettings["Consul"]);
            var clientConfig = new ConsulClientConfiguration()
            {
                Address = consulUrl
            };

            consulClient = new ConsulClient(clientConfig);
        }

       public async Task<bool> putConsul(string key, string value)
        {
            var putPair = new KVPair(key)
            {
                Value = Encoding.UTF8.GetBytes(value)
            };

            var putAttempt = await consulClient.KV.Put(putPair);
            if (putAttempt.Response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PutPairs(string key, string value)
        {
            bool x = putConsul(key, value).GetAwaiter().GetResult();

            return x; 
        }
    }
}