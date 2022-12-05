using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Data;
using PersonalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.ControllerArea
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
            roleManager = roleMgr;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowCreateEmployer()
        {
            return View("CreateEmployer");
        }

        public IActionResult CreateEmployer()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> CreateEmployer(string firstName, string lastName, string email, string password)
        {
            if (ModelState.IsValid)
            {
                string roleName = "Employer";

                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                IdentityUser user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, roleName);

                IdentityUser EmployerUser = await userManager.FindByEmailAsync(email);
                Employer employer = new Employer();

                employer.FirstName = firstName;
                employer.LastName = lastName;
                employer.Email = email;
                employer.UserID = EmployerUser.Id;

                _context.Add(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public IActionResult ShowCreateEmployee()
        {
            var allEmployers = (from Employer in _context.Employer
                              select new SelectListItem()
                              {
                                  Text = Employer.FirstName + " " + Employer.LastName,
                                  Value = Employer.EmployerID.ToString(),
                              }).ToList();
            Employee employee = new Employee();
            employee.EmployerList = allEmployers;

            return View("CreateEmployee", employee);
        }



        [HttpPost]
        public async Task<IActionResult> CreateEmployee(string firstName, string lastName, int employerID, string email, string password)
        {
            if (ModelState.IsValid)
            {
                string roleName = "Employee";

                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                IdentityUser user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, roleName);

                IdentityUser EmployeeUser = await userManager.FindByEmailAsync(email);
                Employee employee = new Employee();

                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.EmployerID = employerID;
                employee.Email = email;
                employee.UserID = EmployeeUser.Id;

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult EmployUser()
        {
            var employeeList = (from Employee in _context.Employee
                             select new SelectListItem()
                             {
                                 Text = Employee.FirstName + " " + Employee.LastName,
                                 Value = Employee.EmployeeID.ToString(),
                             }).ToList();
            Employee employee = new Employee();
            employee.EmployeeList = employeeList;

            var employerList = (from Employer in _context.Employer
                                select new SelectListItem()
                                {
                                    Text = Employer.FirstName + " " + Employer.LastName,
                                    Value = Employer.EmployerID.ToString(),
                                }).ToList();
            employee.EmployerList = employerList;

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployUser([Bind("EmployeeID", "EmployerID")] Employee newEmployee, [Bind("EmployerID")] Employer newEmployer)
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer
                //               .FirstOrDefaultAsync(m => m.UserID == id);
                .FirstOrDefaultAsync(m => m.EmployerID == newEmployer.EmployerID);
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == newEmployee.EmployeeID);

            if (ModelState.IsValid)
            {

                employee.EmployerID = employer.EmployerID;
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();
            }
            return View("Index");
        }
    }
}
