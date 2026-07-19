using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackReminderShownFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FeedbackReminderShowen",
                table: "LessonAssignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 5,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 6,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 7,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 8,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 9,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 10,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 11,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 12,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 13,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 14,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 15,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 16,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 17,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 18,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 19,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 20,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 21,
                column: "FeedbackReminderShowen",
                value: false);

            migrationBuilder.UpdateData(
                table: "LessonAssignments",
                keyColumn: "Id",
                keyValue: 22,
                column: "FeedbackReminderShowen",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedbackReminderShowen",
                table: "LessonAssignments");
        }
    }
}
