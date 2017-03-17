using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace wvDevOps.Helpers
{
    public class AwsWV
    {
        public List<string> GetVersions(string AppName)
        {
            var region = RegionEndpoint.USEast1;
            AmazonEC2Client client = new AmazonEC2Client(region);
            var request = new DescribeImagesRequest()
            {
                Filters = new List<Filter>()
                {
                    new Filter()
                    {
                        Name = "tag:Application",
                        Values= new List<String>()
                        {
                            AppName.ToLower()
                        }
                    }
                }
            };
            var versions = new List<String>();
            var response = client.DescribeImages(request);
            foreach (var ami in response.Images)
            {

                var dictionary = ami.Tags.ToDictionary(item => item.Key, item => item.Value);
                versions.Add(dictionary["AppVersion"]);

            }
            return versions;
        }
    }
}