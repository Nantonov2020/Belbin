using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DbConnectionString")
        {

        }

        public DbSet<Firm> Firms { get; set; }
        public DbSet<HR_Manager> HR_Managers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}