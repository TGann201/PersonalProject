using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Data;
using PersonalProject.Models;

namespace PersonalProject.Controllers
{
    [Authorize(Roles = "Employee, Employer")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employee employee = await _context.Employee.FirstOrDefaultAsync(n => n.UserID == id);
            ViewBag.Name = employee.FirstName + ' ' + employee.LastName;
            //return View(await _context.Employee.ToListAsync());
            return View("Index", employee);
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;


            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,EmployerID,UserID,FirstName,LastName,Email,UserRole")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,EmployerID,UserID,FirstName,LastName,Email,UserRole")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }

        public async Task<IActionResult> EmployeeInfo()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;

            Employee employee = await _context.Employee.FirstOrDefaultAsync(n => n.UserID == id);
            Employer employer = await _context.Employer.FirstOrDefaultAsync(d => d.EmployerID == employee.EmployerID);

            var viewmodel = from e in _context.Employee
                            join p in _context.Position
                            on e.PositionID equals p.PositionID
                            where p.PositionID == employee.PositionID
                            select new EmployeeInfoViewModel { Employees = e, Positions = p };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;

            if (viewmodel != null && employer != null)
            {
                ViewBag.EmployerName = employer.FirstName + ' ' + employer.LastName;
                return View("EmployeeInfo", viewmodel);
            }

            return View("EmployeeInfo");
        }

        public async Task<IActionResult> EmployerEmployeeInfo(int? id)
        {
            Employee employee = await _context.Employee.FirstOrDefaultAsync(m => m.EmployeeID == id);
            Employer employer = await _context.Employer.FirstOrDefaultAsync(d => d.EmployerID == employee.EmployerID);

            var viewmodel = from e in _context.Employee
                            join p in _context.Position
                            on e.PositionID equals p.PositionID
                            where p.PositionID == employee.PositionID
                            select new EmployeeInfoViewModel { Employees = e, Positions = p };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;

            if (viewmodel != null && employer != null)
            {
                ViewBag.EmployerName = employer.FirstName + ' ' + employer.LastName;
                return View("EmployeeInfo", viewmodel);
            }

            return View("EmployeeInfo");
        }

        public async Task<IActionResult> EmployeeSchedule()
        {
            //return View(await _context.Employee.ToListAsync());
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employee employee = await _context.Employee.FirstOrDefaultAsync(n => n.UserID == id);
            Schedule schedule = await _context.Schedule.FirstOrDefaultAsync(d => d.EmployeeID == employee.EmployeeID);

            var viewmodel = from e in _context.Employee
                            join s in _context.Schedule
                            on e.ScheduleID equals s.ScheduleID
                            where s.ScheduleID == employee.ScheduleID
                            select new EmployeeScheduleViewModel { Employees = e, Schedules = s };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;

            if (viewmodel != null)
            {
                return View("EmployeeSchedule", viewmodel);
            }

            return View("EmployeeSchedule");
        }

        public async Task<IActionResult> EmployerEmployeeSchedule(int? id)
        {
            Employee employee = await _context.Employee.FirstOrDefaultAsync(m => m.EmployeeID == id);
            Schedule schedule = await _context.Schedule.FirstOrDefaultAsync(d => d.EmployeeID == employee.EmployeeID);

            var viewmodel = from e in _context.Employee
                            join s in _context.Schedule
                            on e.ScheduleID equals s.ScheduleID
                            where s.ScheduleID == employee.ScheduleID
                            select new EmployeeScheduleViewModel { Employees = e, Schedules = s };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;

            if (viewmodel != null)
            {
                return View("EmployeeSchedule", viewmodel);
            }

            return View("EmployeeSchedule");
        }

        public IActionResult TakeTimeOff()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTimeOff([Bind("EmployerID,EmployeeID,Date,Note")] TimeOff aTimeOff)
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (ModelState.IsValid)
            {
                aTimeOff.EmployeeID = employee.EmployeeID;
                aTimeOff.EmployerID = employee.EmployerID;
                _context.Add(aTimeOff);
                await _context.SaveChangesAsync();

            }
            return View("Index");
        }

        public async Task<IActionResult> ViewTimeOff()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.UserID == id);

            var viewmodel = from e in _context.Employee
                            join t in _context.TimeOff
                            on e.EmployeeID equals t.EmployeeID
                            where e.EmployeeID == employee.EmployeeID
                            where t.Date >= DateTime.Today
                            orderby t.Date ascending
                            select new TimeOffViewModel { Employees = e, TimeOffs = t };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;
            if (viewmodel != null)
            {
                return View(viewmodel);
            }
            return View("ViewTimeOff");
        }

        public async Task<IActionResult> ViewReview()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.UserID == id);
            Employer employer = await _context.Employer
                .FirstOrDefaultAsync(n => n.EmployerID == employee.EmployerID);

            var viewmodel = from e in _context.Employee
                            join p in _context.Performance
                            on e.EmployeeID equals p.EmployeeID
                            where e.EmployeeID == employee.EmployeeID
                            where p.EmployerID == employer.EmployerID
                            select new PerformanceViewModel { Employees = e, Performances = p };

            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;
            ViewBag.EmployerName = employer.FirstName + ' ' + employer.LastName;
            if (viewmodel != null)
            {
                return View(viewmodel);
            }
            return View("ViewReview");
        }
    }
}
