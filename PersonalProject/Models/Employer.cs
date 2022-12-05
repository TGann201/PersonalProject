using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class Employer: User
    {
        public int EmployerID { get; set; }
        public int EmployeeID { get; set; }
    }
}
