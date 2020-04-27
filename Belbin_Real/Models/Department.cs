using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int FirmId { get; set; }
        public string Name { get; set; }
        public virtual Firm Firm { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }

    }
}