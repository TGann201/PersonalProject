using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class Position
    {
        public int PositionID { get; set; }
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float? Salary { get; set; }
        [NotMapped]
        public List<SelectListItem> PositionList { get; set; }
    }
}
