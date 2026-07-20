using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "CreditHours", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(1125), 0, "This is a C# programming course.", "C# Programming" },
                    { 2, new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(1127), 0, "This is an ASP.NET Web API course.", "Asp Dot net Web API" },
                    { 3, new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(1128), 0, "This is a SQL Server course.", "SQL Server" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(951));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(962));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 20, 20, 9, 51, 665, DateTimeKind.Local).AddTicks(964));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 20, 11, 14, 453, DateTimeKind.Local).AddTicks(1084));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 20, 11, 14, 453, DateTimeKind.Local).AddTicks(1094));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 20, 11, 14, 453, DateTimeKind.Local).AddTicks(1095));
        }
    }
}
