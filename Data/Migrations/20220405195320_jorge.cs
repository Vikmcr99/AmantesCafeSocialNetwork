using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class jorge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Coffee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Coffee");
        }
    }
}
