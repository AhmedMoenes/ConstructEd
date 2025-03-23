using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructEd.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteForEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Plugins_PluginId",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Plugins_PluginId",
                table: "Enrollments",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Plugins_PluginId",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Plugins_PluginId",
                table: "Enrollments",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id");
        }
    }
}
