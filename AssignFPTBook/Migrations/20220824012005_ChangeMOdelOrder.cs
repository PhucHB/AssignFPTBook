using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignFPTBook.Migrations
{
    public partial class ChangeMOdelOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "shopID",
                table: "OrderDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shopID",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
