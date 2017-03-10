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
            //var clientConfig = new ConsulClientConfiguration()
            //{
            //    Address = consulUrl
            //};

            consulClient = new ConsulClient((c) => { c.Address = consulUrl; });
        }

        public async Task<bool> putFolder(string key)
        {
            var putPair = new KVPair(key)
            {
                Value = null
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

        public async Task<string>getPair(string key)
        {
            var getPair = await consulClient.KV.Get(key);
            return Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
        }

        public async Task<List<string>> getEnvironments(string key)
        {
            var getList = await consulClient.KV.Keys(key);
            List<String> values = new List<string>();
            if (getList.Response != null)
            {

                foreach (string val in getList.Response)
                {
                    string[] env = val.Split('/');
                    if (env[1] != "")
                    {
                        if (!values.Contains(env[1]))
                        {
                            values.Add(env[1]);
                        }
                    }
                };
            }
            else
            {
                values = null;
            }
          
            return values;

        }
    }
}