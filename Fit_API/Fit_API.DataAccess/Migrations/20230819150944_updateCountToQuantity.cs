using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fit_API.DataAccess.Migrations
{
    public partial class updateCountToQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "CartItem",
                newName: "Quantity");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3f085549-4cc8-4ba3-a0ea-aae8d5dacd9f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6c451667-f1c8-4ebc-9a67-6a24ec77ee41");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CartItem",
                newName: "Count");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "dfd83aff-6c01-4227-a062-5aba01e8b62d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3225471c-e320-4923-8e7b-2e35317478b2");
        }
    }
}
