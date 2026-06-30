using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class SeedLessonAssignmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "Effort", "Inactive", "Position", "Title", "URL" },
                values: new object[,]
                {
                    { 11, 1.0, false, 2, "Introduction to HTML", "#" },
                    { 22, 0.5, false, 3, "CSS Basics", "#" },
                    { 33, 2.0, false, 4, "Ruby: What extend and include do", "#" },
                    { 44, 0.5, false, 5, "Rules of thumb against flaky specs", "#" },
                    { 55, 1.5, false, 6, "How to use API", "#" },
                    { 66, 2.0, false, 7, "Linux", "https://makandracards.com/makandra-devops-curriculum/509339-linux-2-pt" },
                    { 77, 1.0, false, 7, "Linux file system", "https://makandracards.com/makandra-devops-curriculum/511179-linux-filesystems-und-verschluesselung-2-pt" },
                    { 88, 1.0, false, 8, "Resource use", "https://makandracards.com/makandra-devops-curriculum/509415-ressourcen-nutzung-1-pt" },
                    { 99, 0.5, false, 9, "Linux Kernal parameter", "https://makandracards.com/makandra-devops-curriculum/511330-linux-kernel-parameter-0-5-pt" },
                    { 100, 4.0, false, 10, "Network", "https://makandracards.com/makandra-devops-curriculum/509341-netzwerke-4-pt" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Closed", "Email", "EndDate", "HashedPassword", "Name", "Role", "StartingDate" },
                values: new object[] { 1, false, "testtrainee@makandra.de", new DateOnly(2027, 1, 1), "12345", "Test Trainee", "Trainee", new DateOnly(2026, 6, 30) });

            migrationBuilder.InsertData(
                table: "LessonAssignments",
                columns: new[] { "Id", "ExpectedProcessingDate", "LessonId", "Position", "Status", "TraineeId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2026, 6, 24), 11, 2, 3, 1 },
                    { 2, new DateOnly(2026, 6, 25), 22, 3, 3, 1 },
                    { 3, new DateOnly(2026, 6, 26), 33, 4, 5, 1 },
                    { 4, new DateOnly(2026, 6, 26), 44, 5, 3, 1 },
                    { 5, new DateOnly(2026, 6, 27), 55, 6, 2, 1 },
                    { 6, new DateOnly(2026, 6, 28), 66, 7, 2, 1 },
                    { 7, new DateOnly(2026, 6, 30), 77, 8, 1, 1 },
                    { 8, new DateOnly(2026, 6, 30), 88, 9, 1, 1 },
                    { 9, new DateOnly(2026, 7, 1), 99, 10, 0, 1 },
                    { 10, new DateOnly(2026, 7, 1), 100, 11, 0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
