using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructEd.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists");

            migrationBuilder.AddColumn<int>(
                name: "PluginId1",
                table: "Wishlists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PluginId1",
                table: "ShoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PluginId1",
                table: "Enrollments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_PluginId1",
                table: "Wishlists",
                column: "PluginId1");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_PluginId1",
                table: "ShoppingCarts",
                column: "PluginId1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_PluginId1",
                table: "Enrollments",
                column: "PluginId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Plugins_PluginId1",
                table: "Enrollments",
                column: "PluginId1",
                principalTable: "Plugins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId1",
                table: "ShoppingCarts",
                column: "PluginId1",
                principalTable: "Plugins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Plugins_PluginId1",
                table: "Wishlists",
                column: "PluginId1",
                principalTable: "Plugins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Plugins_PluginId1",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId1",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Plugins_PluginId1",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_PluginId1",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_PluginId1",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_PluginId1",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "PluginId1",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "PluginId1",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "PluginId1",
                table: "Enrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Plugins_PluginId",
                table: "ShoppingCarts",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Plugins_PluginId",
                table: "Wishlists",
                column: "PluginId",
                principalTable: "Plugins",
                principalColumn: "Id");
        }
    }
}
