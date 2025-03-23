using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructEd.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactForms",
                table: "ContactForms");

            migrationBuilder.RenameTable(
                name: "ContactForms",
                newName: "ContactForm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactForm",
                table: "ContactForm",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CourseReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseReview_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseReview_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseReview_CourseId",
                table: "CourseReview",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseReview_UserId",
                table: "CourseReview",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactForm",
                table: "ContactForm");

            migrationBuilder.RenameTable(
                name: "ContactForm",
                newName: "ContactForms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactForms",
                table: "ContactForms",
                column: "Id");
        }
    }
}
