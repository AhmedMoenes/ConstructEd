using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructEd.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIsInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInstructor",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInstructor",
                table: "AspNetUsers");
        }
    }
}
