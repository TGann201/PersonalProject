using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonalProject.Models.Position> Position { get; set; }
        public DbSet<PersonalProject.Models.Schedule> Schedule { get; set; }
        public DbSet<PersonalProject.Models.TimeOff> TimeOff { get; set; }
        public DbSet<PersonalProject.Models.Performance> Performance { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Position>().HasData(
                new Position
                {
                    PositionID = 1,
                    PositionName = "Team Member",
                    Salary = 45000
                },
                new Position
                {
                    PositionID = 2,
                    PositionName = "Team Leader",
                    Salary = 50000
                });

            builder.Entity<Schedule>().HasData(
                new Schedule
                {
                    ScheduleID = 1,
                    Shift = 1,
                    StartTime = new TimeSpan(6, 45, 0),
                    EndTime = new TimeSpan(14, 55, 0)
                },
                new Schedule
                {
                    ScheduleID = 2,
                    Shift = 2,
                    StartTime = new TimeSpan(14, 45, 0),
                    EndTime = new TimeSpan(22, 55, 0)
                },
                new Schedule
                {
                    ScheduleID = 3,
                    Shift = 3,
                    StartTime = new TimeSpan(22, 45, 0),
                    EndTime = new TimeSpan(6, 55, 0)
                }
                );
        }

            public static async Task CreateAdministrator(IServiceProvider serviceProvider)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "admin@project.com";
            string password = "Administrator1!";
            string roleName = "Admin";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByEmailAsync(email) == null)
            {
                IdentityUser user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public static async Task CreateEmployer(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "employer@project.com";
            string password = "Employer1!";
            string roleName = "Employer";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByEmailAsync(email) == null)
            {
                IdentityUser user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }

                var employer = new Employer
                {
                    UserID = user.Id,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "employer@project.com"
                };
                context.Add(employer);
                await context.SaveChangesAsync();
            }
        }

        public static async Task CreateEmployee(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "employee@project.com";
            string password = "Employee1!";
            string roleName = "Employee";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByEmailAsync(email) == null)
            {
                IdentityUser user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }

                var employee = new Employee
                {
                    UserID = user.Id,
                    FirstName = "Kevin",
                    LastName = "James",
                    Email = "employee@project.com"
                };
                context.Add(employee);
                await context.SaveChangesAsync();
            }
        }

        public DbSet<PersonalProject.Models.Employer> Employer { get; set; }

        public DbSet<PersonalProject.Models.Employee> Employee { get; set; }
    }
}
