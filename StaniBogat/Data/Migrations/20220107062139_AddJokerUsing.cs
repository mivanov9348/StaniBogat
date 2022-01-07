using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaniBogat.Data.Migrations
{
    public partial class AddJokerUsing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AttendanceUsed",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CallUsed",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FiftyFiftyUsed",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceUsed",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CallUsed",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FiftyFiftyUsed",
                table: "Games");
        }
    }
}
