using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class TimeOffViewModel
    {
        public int ID { get; set; }
        public TimeOff TimeOffs { get; set; }
        public Employer Employers { get; set; }
        public Employee Employees { get; set; }
    }
}
