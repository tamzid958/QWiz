using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QWiz.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17377ee2-8c41-4ec9-97c8-4f12780acc71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c4fcd97-5efc-41b9-8df2-ff2c3b6b5e73");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f82d5874-916f-45d3-8b84-79547e7e1eeb", "d0cb5810-ac69-457e-ad11-d168f29221b8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f82d5874-916f-45d3-8b84-79547e7e1eeb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d0cb5810-ac69-457e-ad11-d168f29221b8");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ced0647-3bf4-4784-ae9d-6a280b6956a6", "75f35e7a-4e3a-49fd-9936-ef39a0a83b1a", "QuestionSetter", "QUESTIONSETTER" },
                    { "9c339dfd-4142-4949-b170-a3592ec82c98", "9c339dfd-4142-4949-b170-a3592ec82c98", "Admin", "ADMIN" },
                    { "f3f73849-88b6-4773-a4ae-e65d789b5937", "47f59542-2846-406d-bdc4-d8c580e2c54f", "Reviewer", "REVIEWER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "75d1047d-bba1-4df0-93d5-08e7b728c741", 0, "2084849c-367b-4850-bb54-3a5ae4dbf2aa", "tamjidahmed958@gmail.com", true, "Tamzid Ahmed", false, null, null, "TAMZID", "AQAAAAIAAYagAAAAEMnx6xJ7UMIZ1gLJsoAbeWQLUkcBOYRXkV54wiZiFd0nDoGTYVeeyOSWJI0kLBzGmA==", "01521203280", false, null, new DateTime(2024, 4, 27, 20, 45, 5, 629, DateTimeKind.Local).AddTicks(9150), "ac6d1cfa-3ed6-451e-af98-26bce32e05f1", false, "tamzid" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9c339dfd-4142-4949-b170-a3592ec82c98", "75d1047d-bba1-4df0-93d5-08e7b728c741" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ced0647-3bf4-4784-ae9d-6a280b6956a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3f73849-88b6-4773-a4ae-e65d789b5937");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9c339dfd-4142-4949-b170-a3592ec82c98", "75d1047d-bba1-4df0-93d5-08e7b728c741" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c339dfd-4142-4949-b170-a3592ec82c98");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "75d1047d-bba1-4df0-93d5-08e7b728c741");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17377ee2-8c41-4ec9-97c8-4f12780acc71", "72844698-2534-4a79-a707-eccc6d4ea342", "QuestionSetter", "QUESTIONSETTER" },
                    { "7c4fcd97-5efc-41b9-8df2-ff2c3b6b5e73", "674fbeae-7e57-4cae-8277-948464cfc33c", "Reviewer", "REVIEWER" },
                    { "f82d5874-916f-45d3-8b84-79547e7e1eeb", "f82d5874-916f-45d3-8b84-79547e7e1eeb", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d0cb5810-ac69-457e-ad11-d168f29221b8", 0, "9386a6af-bd8c-48a7-9310-b6aca86069c9", "tamjidahmed958@gmail.com", true, "Tamzid Ahmed", false, null, null, "TAMZID", "AQAAAAIAAYagAAAAEFhlY6pabRQuTLPb3QdMDpQIJ3CUgvMEmS8qULwdHpjEwBC9y3E/d7bfXddxPyiXCQ==", "01521203280", false, "efe16dcb-2d24-427b-b365-65ec57dacfa3", false, "tamzid" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f82d5874-916f-45d3-8b84-79547e7e1eeb", "d0cb5810-ac69-457e-ad11-d168f29221b8" });
        }
    }
}
