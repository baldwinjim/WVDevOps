using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wvDevOps.Models
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<Env> Envs { get; set; }
    }
}