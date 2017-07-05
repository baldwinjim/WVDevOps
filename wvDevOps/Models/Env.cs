using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wvDevOps.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Table("environments")]
    public class Env
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cidr { get; set; }
        public string Region { get; set; }
        public bool ProtectedEnv { get; set; }
        public DateTime Updated { get; set; }
    }
}