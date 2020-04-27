using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public string DateRez { get; set; }
        public string Results { get; set; }

        public virtual Worker Worker { get; set; }

    }
}