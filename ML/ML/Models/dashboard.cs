using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Models
{
    public class dashboard
    {
        public virtual announcement announcement { get; set; }
        public List<announcement> announcements { get; set; }
        public List<PcProblem> PcProblems { get; set; }
    }
}