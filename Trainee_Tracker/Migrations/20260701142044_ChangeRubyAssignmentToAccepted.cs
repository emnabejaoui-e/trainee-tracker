using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRubyAssignmentToAccepted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: 5);
        }
    }
}
