using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentAndLessonToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "LessonFeedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "LessonFeedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LessonFeedbacks_LessonId",
                table: "LessonFeedbacks",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonFeedbacks_Lessons_LessonId",
                table: "LessonFeedbacks",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonFeedbacks_Lessons_LessonId",
                table: "LessonFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_LessonFeedbacks_LessonId",
                table: "LessonFeedbacks");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "LessonFeedbacks");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "LessonFeedbacks");
        }
    }
}
