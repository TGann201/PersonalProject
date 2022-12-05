using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalProject.Data.Migrations
{
    public partial class AddedPerformance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PerformanceID",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Performance",
                columns: table => new
                {
                    PerformanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(nullable: false),
                    EmployerID = table.Column<int>(nullable: false),
                    LearnValue = table.Column<int>(nullable: false),
                    LearnNote = table.Column<string>(nullable: true),
                    AdaptabilityValue = table.Column<int>(nullable: false),
                    AdaptabilityNote = table.Column<string>(nullable: true),
                    AttendanceValue = table.Column<int>(nullable: false),
                    AttendanceNote = table.Column<string>(nullable: true),
                    TeamworkValue = table.Column<int>(nullable: false),
                    TeamworkNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance", x => x.PerformanceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performance");

            migrationBuilder.DropColumn(
                name: "PerformanceID",
                table: "Employee");
        }
    }
}
