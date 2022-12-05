using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalProject.Data.Migrations
{
    public partial class AddedPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionID",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    PositionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(nullable: true),
                    Salary = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.PositionID);
                });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "PositionID", "PositionName", "Salary" },
                values: new object[] { 1, "Team Member", 45000f });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "PositionID", "PositionName", "Salary" },
                values: new object[] { 2, "Team Leader", 50000f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Employee");
        }
    }
}
