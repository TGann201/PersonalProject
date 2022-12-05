using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class PerformanceViewModel
    {
        public int ID { get; set; }
        public Performance Performances { get; set; }
        public Employee Employees { get; set; }
    }
}
