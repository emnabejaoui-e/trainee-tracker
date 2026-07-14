using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMentorFromLessonFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonFeedbacks_Users_MentorId",
                table: "LessonFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_LessonFeedbacks_MentorId",
                table: "LessonFeedbacks");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "LessonFeedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "LessonFeedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LessonFeedbacks_MentorId",
                table: "LessonFeedbacks",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonFeedbacks_Users_MentorId",
                table: "LessonFeedbacks",
                column: "MentorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
