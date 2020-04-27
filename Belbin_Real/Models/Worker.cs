using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int Boss { get; set; }
        public int Candidate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}