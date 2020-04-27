using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class Firm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<HR_Manager> HR_Managers { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

    }
}