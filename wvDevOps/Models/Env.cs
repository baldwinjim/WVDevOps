using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wvDevOps.Helpers;

namespace wvDevOps.Models
{
    public class Apps
    {
        public string  appName { get; set; }
        public string version { get; set; }
        public string asg_ami { get; set; }
        public string asg_instance_type { get; set; }
        public int asg_min { get; set; }
        public int asg_max { get; set; }
        public int asg_want { get; set; }
        public string cb_instance_type { get; set; }
        public int cb_count { get; set; }
        public string cb_ami { get; set; }
        public string cb_version { get; set; }
        public int indserver_count { get; set; }
        public int lbserver_count { get; set; }
        public string lbserver_instance_type { get; set; }
        public string domainname { get; set; }
        public string hostname { get; set; }
        public string app_sg { get; set; }
        public string app_lb_map { get; set; }
    }

    public class Env
    {
        public string name { get; set; }
        public string vpc_cidr { get; set; }
        public string aws_region { get; set; }
        public bool protectedEnv { get; set; }
        public IEnumerable<Apps> app { get; set; }

    }

}