using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingHourRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingHourRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TraineeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    WorkingHours = table.Column<double>(type: "REAL", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHourRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHourRecords_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHourRecords_TraineeId_Date",
                table: "WorkingHourRecords",
                columns: new[] { "TraineeId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingHourRecords");
        }
    }
}
