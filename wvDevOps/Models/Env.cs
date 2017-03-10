using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wvDevOps.Models
{
    public class Env
    {
        public string name { get; set; }
        public string vpc_cidr { get; set; }
        public string aws_region { get; set; }
    }

}