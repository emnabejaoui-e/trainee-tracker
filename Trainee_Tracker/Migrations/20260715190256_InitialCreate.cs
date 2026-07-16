using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Trainee_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curricula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curricula", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    Effort = table.Column<double>(type: "REAL", nullable: false),
                    Inactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    CurriculumId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Curricula_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    HashedPassword = table.Column<string>(type: "TEXT", nullable: false),
                    Closed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    CurriculumId = table.Column<int>(type: "INTEGER", nullable: true),
                    StartingDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Curricula_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpectedProcessingDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TraineeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonAssignments_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAssignments_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: true),
                    PriorKnowledge = table.Column<string>(type: "TEXT", nullable: true),
                    ActualEffort = table.Column<double>(type: "REAL", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TraineeId = table.Column<int>(type: "INTEGER", nullable: false),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonFeedbacks_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonFeedbacks_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorTrainee",
                columns: table => new
                {
                    AssignedTraineesId = table.Column<int>(type: "INTEGER", nullable: false),
                    MentorsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorTrainee", x => new { x.AssignedTraineesId, x.MentorsId });
                    table.ForeignKey(
                        name: "FK_MentorTrainee_Users_AssignedTraineesId",
                        column: x => x.AssignedTraineesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorTrainee_Users_MentorsId",
                        column: x => x.MentorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rejections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    RejectedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rejections_LessonAssignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "LessonAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Curricula",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "makandra Curriculum" },
                    { 2, "makandra DevOps-Curriculum" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Closed", "Email", "EndDate", "HashedPassword", "Name", "Role", "StartingDate" },
                values: new object[,]
                {
                    { 1, false, "torstentrainee@makandra.de", new DateOnly(2027, 1, 1), "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", "Torsten Trainee", "Trainee", new DateOnly(2026, 6, 30) },
                    { 2, false, "jelenacosic3@makandra.de", new DateOnly(1, 1, 1), "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", "Jelena3 Trainee", "Trainee", new DateOnly(1, 1, 1) },
                    { 3, false, "tildatrainee@makandra.de", new DateOnly(2027, 1, 1), "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", "Tilda Trainee", "Trainee", new DateOnly(2026, 7, 1) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Closed", "CurriculumId", "Email", "HashedPassword", "Name", "Role" },
                values: new object[,]
                {
                    { 5, false, null, "jelenacosic1@makandra.de", "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", "Jelena3 Admin", "Admin" },
                    { 6, false, null, "admin@makandra.de", "$2a$11$NWoCmWYUtc4Kj0eDILuyxOjWj0GReHhxe2bh6Crx1QR4heeWH1EcO", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Closed", "Email", "EndDate", "HashedPassword", "Name", "Role", "StartingDate" },
                values: new object[,]
                {
                    { 9, false, "vanessa.vital@makandra.de", new DateOnly(2027, 3, 31), "$2a$11$OvYPz8FkuxXJe7WyPIHpzOc1bi5beKtsB2WYXBJlVDqsNRCJatfzK", "Vanessa Vital", "Trainee", new DateOnly(2026, 7, 17) },
                    { 10, false, "stefan.schnupfen@makandra.de", new DateOnly(2026, 10, 31), "$2a$11$1YdXfUYVKPO7t0wmYKVirOq4mYR4k/sxxO6YlY7c2CdSO50yRSqGW", "Stefan Schnupfen", "Trainee", new DateOnly(2026, 5, 1) },
                    { 11, false, "ursula.urlaub@makandra.de", new DateOnly(2026, 7, 31), "$2a$11$aemTP4KrL44P1z23XD39u.7nnd3zoXeME0PFZzVkQPTEmmK/fVqRm", "Ursula Urlaub", "Trainee", new DateOnly(2026, 1, 1) }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "CurriculumId", "Effort", "Inactive", "Position", "Title", "URL" },
                values: new object[,]
                {
                    { 11, 1, 3.5, false, 1, "Fundamentals of Web Development", "https://makandracards.com/makandra-devops-curriculum/509333-grundlagen-aus-der-web-entwicklung-3-5-pt" },
                    { 22, 1, 2.0, false, 3, "Virtualization", "https://makandracards.com/makandra-devops-curriculum/509340-virtualisierung-2-pt" },
                    { 33, 1, 2.0, false, 4, "Lxc/LXD", "https://makandracards.com/makandra-devops-curriculum/517382-lxc-lxd-2-pt" },
                    { 44, 1, 1.0, false, 5, "A Brief Introduction to Docker and Containers ", "https://makandracards.com/makandra-devops-curriculum/523491-kurze-einfuehrung-docker-und-container-1-pt" },
                    { 55, 1, 0.5, false, 6, "Firewalling with iptables", "https://makandracards.com/makandra-devops-curriculum/531475-firewalling-mit-iptables-0-5-pt" },
                    { 66, 1, 2.0, false, 2, "Linux", "https://makandracards.com/makandra-devops-curriculum/509339-linux-2-pt" },
                    { 77, 1, 1.0, false, 7, "Linux file system", "https://makandracards.com/makandra-devops-curriculum/511179-linux-filesystems-und-verschluesselung-2-pt" },
                    { 88, 1, 1.0, false, 8, "Resource use", "https://makandracards.com/makandra-devops-curriculum/509415-ressourcen-nutzung-1-pt" },
                    { 99, 1, 0.5, false, 9, "Linux Kernal parameter", "https://makandracards.com/makandra-devops-curriculum/511330-linux-kernel-parameter-0-5-pt" },
                    { 100, 1, 4.0, false, 10, "Network", "https://makandracards.com/makandra-devops-curriculum/509341-netzwerke-4-pt" },
                    { 111, 1, 0.5, false, 6, "SSH", "https://makandracards.com/makandra-devops-curriculum/511181-ssh-0-5-pt" },
                    { 222, 1, 2.5, false, 11, "HTTP Protocoll and Webserver", "https://makandracards.com/makandra-devops-curriculum/519412-http-protokoll-und-webserver-2-5-pt" }
                });

            migrationBuilder.InsertData(
                table: "MentorTrainee",
                columns: new[] { "AssignedTraineesId", "MentorsId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Closed", "CurriculumId", "Email", "HashedPassword", "Name", "Role" },
                values: new object[,]
                {
                    { 4, false, 1, "jelenacosic2@makandra.de", "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", "Jelena2 Mentor", "Mentor" },
                    { 7, false, 1, "manfred.mental@makandra.de", "$2a$11$kce.fXXVmBy2n0DaoYUcuujmpl.lXCCgZC7WSoFT94B98q5FS.gMa", "Manfred Mental", "Mentor" },
                    { 8, false, 1, "hans.hilfreich@makandra.de", "$2a$11$RirFsrHzwqEKO0Wr8sIiZuprcJ5Pz8y45bRGLltqSos0cePlhhLvC", "Hans Hilfreich", "Mentor" }
                });

            migrationBuilder.InsertData(
                table: "LessonAssignments",
                columns: new[] { "Id", "ExpectedProcessingDate", "LessonId", "Position", "Status", "TraineeId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2026, 6, 29), 11, 2, 3, 1 },
                    { 2, new DateOnly(2026, 6, 29), 22, 3, 3, 1 },
                    { 3, new DateOnly(2026, 6, 29), 33, 4, 3, 1 },
                    { 4, new DateOnly(2026, 6, 30), 44, 5, 3, 1 },
                    { 5, new DateOnly(2026, 6, 30), 55, 6, 2, 1 },
                    { 6, new DateOnly(2026, 7, 1), 66, 7, 2, 1 },
                    { 7, new DateOnly(2026, 7, 1), 77, 8, 1, 1 },
                    { 8, new DateOnly(2026, 7, 1), 88, 9, 1, 1 },
                    { 9, new DateOnly(2026, 7, 2), 99, 10, 0, 1 },
                    { 10, new DateOnly(2026, 7, 3), 100, 11, 0, 3 },
                    { 11, new DateOnly(2026, 6, 29), 11, 2, 3, 3 },
                    { 12, new DateOnly(2026, 6, 29), 22, 3, 3, 3 },
                    { 13, new DateOnly(2026, 6, 29), 33, 4, 3, 3 },
                    { 14, new DateOnly(2026, 6, 30), 44, 6, 3, 3 },
                    { 15, new DateOnly(2026, 6, 30), 55, 5, 2, 3 },
                    { 16, new DateOnly(2026, 7, 1), 66, 7, 2, 3 },
                    { 17, new DateOnly(2026, 7, 1), 77, 9, 2, 3 },
                    { 18, new DateOnly(2026, 7, 2), 88, 8, 1, 3 },
                    { 19, new DateOnly(2026, 7, 2), 99, 10, 1, 3 },
                    { 20, new DateOnly(2026, 7, 3), 100, 11, 0, 3 },
                    { 21, new DateOnly(2026, 7, 3), 111, 12, 0, 3 },
                    { 22, new DateOnly(2026, 7, 3), 222, 13, 0, 3 }
                });

            migrationBuilder.InsertData(
                table: "MentorTrainee",
                columns: new[] { "AssignedTraineesId", "MentorsId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 9, 7 },
                    { 10, 7 },
                    { 11, 7 },
                    { 11, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssignments_LessonId",
                table: "LessonAssignments",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssignments_TraineeId",
                table: "LessonAssignments",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonFeedbacks_LessonId",
                table: "LessonFeedbacks",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonFeedbacks_TraineeId",
                table: "LessonFeedbacks",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CurriculumId",
                table: "Lessons",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTrainee_MentorsId",
                table: "MentorTrainee",
                column: "MentorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejections_AssignmentId",
                table: "Rejections",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurriculumId",
                table: "Users",
                column: "CurriculumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonFeedbacks");

            migrationBuilder.DropTable(
                name: "MentorTrainee");

            migrationBuilder.DropTable(
                name: "Rejections");

            migrationBuilder.DropTable(
                name: "LessonAssignments");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Curricula");
        }
    }
}
