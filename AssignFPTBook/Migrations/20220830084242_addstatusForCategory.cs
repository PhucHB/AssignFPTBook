using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignFPTBook.Migrations
{
    public partial class addstatusForCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Categories");
        }
    }
}
