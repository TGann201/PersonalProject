using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalProject.Data;
using PersonalProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Controllers
{
    [Authorize(Roles = "Employer, Employee")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ApplicationDbContext context, RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr)
        {
            applicationDbContext = context;
            userManager = userMgr;
            roleManager = roleMgr;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            };
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Employers");
            };
            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Employees");
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CompanyNews()
        {
            return View();
        }
    }
}
