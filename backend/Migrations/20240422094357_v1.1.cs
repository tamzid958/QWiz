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
                keyValue: "2c115c7c-46e0-4809-b641-fd657eeda382");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7428720f-870e-4856-92c6-cf4b136d51f7");

            migrationBuilder.AddColumn<bool>(
                name: "IsAddedToQuestionBank",
                table: "Questions",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "800c0842-b1db-4282-9afc-d8c28dd655e7", null, "Reviewer", "REVIEWER" },
                    { "dcd9569f-48e5-49af-a63d-7ae92a2be76f", null, "QuestionSetter", "QUESTIONSETTER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8256a43-f378-4512-99f2-50e0fbd36177", "AQAAAAIAAYagAAAAEINlkbo3JReoNzHPWR1ol9NLFM2pu86u7H3CitkO0y2F1XKEnWMbED02YtqGot7JyQ==", "b70e5c3f-e351-464b-aeaa-bd13870b12ad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "800c0842-b1db-4282-9afc-d8c28dd655e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcd9569f-48e5-49af-a63d-7ae92a2be76f");

            migrationBuilder.DropColumn(
                name: "IsAddedToQuestionBank",
                table: "Questions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c115c7c-46e0-4809-b641-fd657eeda382", null, "QuestionSetter", "QUESTIONSETTER" },
                    { "7428720f-870e-4856-92c6-cf4b136d51f7", null, "Reviewer", "REVIEWER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b03704d-fffc-4f04-925e-226c3f03b48c", "AQAAAAIAAYagAAAAEPLf8GMV/bkUL/NESw2Ev1t+QtXgRPdFyTVDEsvY8urxBN4Ulyu/K27UUgEkQUn8qw==", "0728a9e8-08c0-4825-9451-c33ee6560c31" });
        }
    }
}
