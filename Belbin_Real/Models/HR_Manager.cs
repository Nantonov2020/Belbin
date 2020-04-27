using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class HR_Manager
    {
        public int Id { get; set; }
        public int FirmId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual Firm Firm { get; set; }
     }
}