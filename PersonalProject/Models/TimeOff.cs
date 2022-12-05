using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class TimeOff
    {
        [Key]
        public int TimeOffID { get; set; }

        [DisplayName("Employer")]
        public int EmployerID { get; set; }
        [DisplayName("Employee")]
        public int EmployeeID { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Must pick a date")]
        [DisplayName("Date Requested")]
        public DateTime Date { get; set; }
        [DisplayName("Add Note")]
        public string Note { get; set; }
    }
}
