using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructEd.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Courses_CourseId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Courses_CourseId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "PluginId",
                table: "Wishlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Wishlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PluginId",
                table: "ShoppingCarts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "ShoppingCarts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Enrollments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "PaymentCourse",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCourse", x => new { x.PaymentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_PaymentCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentCourse_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCourse_CourseId",
                table: "PaymentCourse",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Courses_CourseId",
                table: "ShoppingCarts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Courses_CourseId",
                table: "Wishlists",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Courses_CourseId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Courses_CourseId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists");

            migrationBuilder.DropTable(
                name: "PaymentCourse");

            migrationBuilder.AlterColumn<int>(
                name: "PluginId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PluginId",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Courses_CourseId",
                table: "ShoppingCarts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Courses_CourseId",
                table: "Wishlists",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
