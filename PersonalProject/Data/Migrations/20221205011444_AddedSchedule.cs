using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalProject.Data.Migrations
{
    public partial class AddedSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleID",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(nullable: false),
                    Shift = table.Column<int>(nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleID);
                });

            migrationBuilder.InsertData(
                table: "Schedule",
                columns: new[] { "ScheduleID", "EmployeeID", "EndTime", "Shift", "StartTime" },
                values: new object[] { 1, 0, new TimeSpan(0, 14, 55, 0, 0), 1, new TimeSpan(0, 6, 45, 0, 0) });

            migrationBuilder.InsertData(
                table: "Schedule",
                columns: new[] { "ScheduleID", "EmployeeID", "EndTime", "Shift", "StartTime" },
                values: new object[] { 2, 0, new TimeSpan(0, 22, 55, 0, 0), 2, new TimeSpan(0, 14, 45, 0, 0) });

            migrationBuilder.InsertData(
                table: "Schedule",
                columns: new[] { "ScheduleID", "EmployeeID", "EndTime", "Shift", "StartTime" },
                values: new object[] { 3, 0, new TimeSpan(0, 6, 55, 0, 0), 3, new TimeSpan(0, 22, 45, 0, 0) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleID",
                table: "Employee");
        }
    }
}
