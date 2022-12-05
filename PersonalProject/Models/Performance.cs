using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class Performance
    {
        public int PerformanceID { get; set; }
        public int EmployeeID { get; set; }
        public int EmployerID { get; set; }
        [Required(ErrorMessage = "Must pick a value")]
        [Display(Name = "Value")]
        public int LearnValue { get; set; }
        [Display(Name = "Note")]
        public string LearnNote { get; set; }
        [Required(ErrorMessage = "Must pick a value")]
        [Display(Name = "Value")]
        public int AdaptabilityValue { get; set; }
        [Display(Name = "Note")]
        public string AdaptabilityNote { get; set; }
        [Required(ErrorMessage = "Must pick a value")]
        [Display(Name = "Value")]
        public int AttendanceValue { get; set; }
        [Display(Name = "Note")]
        public string AttendanceNote { get; set; }
        [Required(ErrorMessage = "Must pick a value")]
        [Display(Name = "Value")]
        public int TeamworkValue { get; set; }
        [Display(Name = "Note")]
        public string TeamworkNote { get; set; }
    }
}
