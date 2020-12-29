using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskList.Data.Migrations
{
    public partial class AddTaskTimeExecution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfExecution",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TimeOfExecution",
                table: "Tasks",
                type: "int",
                nullable: true);
        }
    }
}
