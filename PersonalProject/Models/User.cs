using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class User
    {
        [Display(Name = "User ID")]
        public string UserID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "User Email")]
        public string Email { get; set; }
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
    }
}
