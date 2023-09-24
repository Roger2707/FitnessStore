using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fit_API.DataAccess.Migrations
{
    public partial class updateShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f64d98d0-6cb3-4cf8-91bf-21fe05b26b5c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "55c94371-a366-4764-8430-b178d4d239d2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
