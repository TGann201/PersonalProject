using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class Employee: User
    {
        public int EmployeeID { get; set; }
        public int EmployerID { get; set; }
        public int PositionID { get; set; }
        public int ScheduleID { get; set; }
        public int PerformanceID { get; set; }
        [NotMapped]
        public List<SelectListItem> EmployerList { get; set; }
        [NotMapped]
        public List<SelectListItem> EmployeeList { get; set; }
        [NotMapped]
        public List<SelectListItem> PositionList { get; set; }
        [NotMapped]
        public List<SelectListItem> ScheduleList { get; set; }
    }
}
