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
    [Authorize(Roles = "Employer")]
    public class EmployersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public EmployersController(ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        // GET: Employers
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer.FirstOrDefaultAsync(n => n.UserID == id);
            ViewBag.Name = employer.FirstName + ' ' + employer.LastName;
            //return View(await _context.Employee.ToListAsync());
            return View("Index", employer);
        }

        // GET: Employers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer
                .FirstOrDefaultAsync(m => m.EmployerID == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // GET: Employers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployerID,UserID,FirstName,LastName,Email,UserRole")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employer);
        }

        // GET: Employers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployerID,UserID,FirstName,LastName,Email,UserRole")] Employer employer)
        {
            if (id != employer.EmployerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerExists(employer.EmployerID))
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
            return View(employer);
        }

        // GET: Employers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer
                .FirstOrDefaultAsync(m => m.EmployerID == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employer = await _context.Employer.FindAsync(id);
            _context.Employer.Remove(employer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerExists(int id)
        {
            return _context.Employer.Any(e => e.EmployerID == id);
        }

        public async Task<IActionResult> EmployeeList()
        {
            //return View(await _context.Employee.ToListAsync());
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer.FirstOrDefaultAsync(n => n.UserID == id);
            Employee employee = await _context.Employee.FirstOrDefaultAsync(d => d.EmployerID == employer.EmployerID);

            var viewmodel = from e in _context.Employer
                            join a in _context.Employee
                            on e.EmployerID equals a.EmployerID
                            where a.EmployerID == employer.EmployerID
                            select new EmployeeListViewModel { Employees = a, Employers = e };

            if (employee != null)
            {
                ViewBag.Name = employer.FirstName + ' ' + employer.LastName;
                return View("EmployeeList", viewmodel);
            }

            return View("EmployeeList");
        }

        public IActionResult AssignPosition()
        {
            var employeeList = (from Employee in _context.Employee
                                select new SelectListItem()
                                {
                                    Text = Employee.FirstName + " " + Employee.LastName,
                                    Value = Employee.EmployeeID.ToString(),
                                }).ToList();
            Employee employee = new Employee();
            employee.EmployeeList = employeeList;

            var positionList = (from Position in _context.Position
                                select new SelectListItem()
                                {
                                    Text = Position.PositionName,
                                    Value = Position.PositionID.ToString(),
                                }).ToList();
            employee.PositionList = positionList;

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssignPosition([Bind("EmployeeID", "PositionID")] Employee aEmployee, [Bind("PositionID")] Employer aPosition)
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer
                //               .FirstOrDefaultAsync(m => m.UserID == id);
                .FirstOrDefaultAsync(m => m.UserID == id);
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == aEmployee.EmployeeID);
            Position position = await _context.Position
                .FirstOrDefaultAsync(m => m.PositionID == aEmployee.PositionID);

            if (ModelState.IsValid)
            {
                employee.PositionID = position.PositionID;
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();
            }
            return View("Index");
        }

        public IActionResult AssignSchedule()
        {
            var employeeList = (from Employee in _context.Employee
                                select new SelectListItem()
                                {
                                    Text = Employee.FirstName + " " + Employee.LastName,
                                    Value = Employee.EmployeeID.ToString(),
                                }).ToList();
            Employee employee = new Employee();
            employee.EmployeeList = employeeList;

            var scheduleList = (from Schedule in _context.Schedule
                                select new SelectListItem()
                                {
                                    Text = Schedule.Shift.ToString(),
                                    Value = Schedule.ScheduleID.ToString(),
                                }).ToList();
            employee.ScheduleList = scheduleList;

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssignSchedule([Bind("EmployeeID", "ScheduleID")] Employee aEmployee, [Bind("ScheduleID")] Schedule aSchedule)
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer
                //               .FirstOrDefaultAsync(m => m.UserID == id);
                .FirstOrDefaultAsync(m => m.UserID == id);
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == aEmployee.EmployeeID);
            Schedule schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.ScheduleID == aEmployee.ScheduleID);

            if (ModelState.IsValid)
            {
                employee.ScheduleID = schedule.ScheduleID;
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();
            }
            return View("Index");
        }

        public async Task<IActionResult> ViewEmployeeTimeOff()
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer.FirstOrDefaultAsync(d => d.UserID == id);

            var viewmodel = from e in _context.Employee
                            join t in _context.TimeOff
                            on e.EmployeeID equals t.EmployeeID
                            where e.EmployerID == employer.EmployerID
                            select new TimeOffViewModel { Employees = e, TimeOffs = t };
            ViewBag.EmployerName = employer.FirstName + ' ' + employer.LastName;
            if (viewmodel != null)
            {
                return View(viewmodel);
            }
            return View("ViewEmployeeTimeOff");
        }

        public async Task<IActionResult> EmployeeReview(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            var idA = user.Id;
            Employer employer = await _context.Employer
                .FirstOrDefaultAsync(m => m.UserID == idA);
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            var employeeList = (from Employee in _context.Employee
                                where Employee.EmployerID == employer.EmployerID
                                select new SelectListItem()
                                {
                                    Text = Employee.FirstName + " " + Employee.LastName,
                                    Value = Employee.EmployeeID.ToString(),
                                }).ToList();
            var learnList = (from Employee in _context.Employee
                                where Employee.EmployerID == employer.EmployerID
                                select new SelectListItem()
                                {
                                    Text = Employee.FirstName + " " + Employee.LastName,
                                    Value = Employee.EmployeeID.ToString(),
                                }).ToList();
            employee.EmployeeList = employeeList;

            //var viewmodel = from e in _context.Employee
            //                join p in _context.Performance
            //                on e.EmployeeID equals p.EmployeeID
            //                where e.EmployerID == employer.EmployerID
            //                select new PerformanceViewModel { Employees = e, Performances = p };
            ViewBag.EmployeeName = employee.FirstName + ' ' + employee.LastName;
            //if (viewmodel != null)
            //{
            //    return View(viewmodel);
            //}
            return View("EmployeeReview");

            //return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeReview([Bind("EmployeeID", "PerformanceID")] Employee aEmployee, [Bind("EmployeeID,EmployerID,LearnValue,LearnNote,AdaptabilityValue,AdaptabilityNote,AttendanceValue,AttendanceNote,TeamworkValue,TeamworkNote")] Performance aPerformance)
        {
            var user = await userManager.GetUserAsync(User);
            var id = user.Id;
            Employer employer = await _context.Employer
                //               .FirstOrDefaultAsync(m => m.UserID == id);
                .FirstOrDefaultAsync(m => m.UserID == id);
            Employee employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployerID == employer.EmployerID);
            //Performance performance = await _context.Performance
            //    .FirstOrDefaultAsync(m => m.EmployeeID == aEmployee.EmployeeID);

            if (ModelState.IsValid)
            {
                aPerformance.EmployerID = employer.EmployerID;
                aPerformance.EmployeeID = employee.EmployeeID;
                _context.Add(aPerformance);
                await _context.SaveChangesAsync();

                Performance performance = await _context.Performance
                    .FirstOrDefaultAsync(m => m.EmployeeID == employee.EmployeeID);

                employee.PerformanceID = performance.PerformanceID;
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();
            }
            return View("Index");
        }
    }
}
