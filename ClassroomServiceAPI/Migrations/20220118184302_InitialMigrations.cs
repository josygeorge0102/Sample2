using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomServiceAPI.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    ClassroomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.ClassroomId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "StudentClassroomMappers",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassroomMappers", x => new { x.ClassroomId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentClassroomMappers_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    TestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.UniqueConstraint("AK_Tests_TestId_QuestionId", x => new { x.TestId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_Tests_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tests_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    StudentAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentScore = table.Column<int>(type: "int", nullable: false),
                    TestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "ClassroomId", "ImageUrl", "Name", "Subject", "TeacherId" },
                values: new object[,]
                {
                    { 1, "example.com/myImage", "Suyog Classroom", "suyos Fav. Subject", 1 },
                    { 2, "example.com/myImage", "Shuchi Classroom", "shuchi Fav. Subject", 2 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "CorrectAnswer", "OptionA", "OptionB", "OptionC", "OptionD", "Subject", "TeacherId", "Title" },
                values: new object[,]
                {
                    { 1, "C#", "C#", "Typescript", "Javascript", "C", "suyos Fav. Subject", 1, "What is my Fav. Language" },
                    { 2, "javascript", "C#", "Typescript", "Javascript", "C", "shuchi Fav. Subject", 1, "What is my Fav. Language" }
                });

            migrationBuilder.InsertData(
                table: "StudentClassroomMappers",
                columns: new[] { "ClassroomId", "StudentId" },
                values: new object[,]
                {
                    { 1, 1234 },
                    { 2, 5678 }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "ClassroomId", "Name", "QuestionId", "TestDate", "TestId" },
                values: new object[,]
                {
                    { 1, 1, "My new test", 1, new DateTimeOffset(new DateTime(2022, 1, 18, 18, 43, 2, 378, DateTimeKind.Unspecified).AddTicks(784), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 2, 1, "My very new test", 2, new DateTimeOffset(new DateTime(2022, 1, 18, 18, 43, 2, 378, DateTimeKind.Unspecified).AddTicks(795), new TimeSpan(0, 0, 0, 0, 0)), 1 }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "ClassroomId", "QuestionId", "StudentAnswer", "StudentId", "StudentScore", "TestDate", "TestId" },
                values: new object[] { 1, 1, 1, "C#", 4, 5, new DateTimeOffset(new DateTime(2022, 1, 18, 18, 43, 2, 378, DateTimeKind.Unspecified).AddTicks(828), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "ClassroomId", "QuestionId", "StudentAnswer", "StudentId", "StudentScore", "TestDate", "TestId" },
                values: new object[] { 2, 2, 2, "javascript", 5, 10, new DateTimeOffset(new DateTime(2022, 1, 18, 18, 43, 2, 378, DateTimeKind.Unspecified).AddTicks(830), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Results_ClassroomId",
                table: "Results",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuestionId",
                table: "Results",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_TestId",
                table: "Results",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ClassroomId",
                table: "Tests",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_QuestionId",
                table: "Tests",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "StudentClassroomMappers");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
