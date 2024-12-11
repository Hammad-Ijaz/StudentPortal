using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddInternalMarksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalMarks",
                columns: table => new
                {
                    MarksId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Course_Id = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TotalMarks = table.Column<float>(type: "real", nullable: false),
                    ObtainedMarks = table.Column<float>(type: "real", nullable: false),
                    TotalResult = table.Column<float>(type: "real", nullable: false),
                    TakingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMarks", x => x.MarksId);
                    table.ForeignKey(
                        name: "FK_InternalMarks_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId");
                    table.ForeignKey(
                        name: "FK_InternalMarks_Courserecord_Course_Id",
                        column: x => x.Course_Id,
                        principalTable: "Courserecord",
                        principalColumn: "Course_Id");
                    table.ForeignKey(
                        name: "FK_InternalMarks_Studentslist_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studentslist",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalMarks_ClassId",
                table: "InternalMarks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMarks_Course_Id",
                table: "InternalMarks",
                column: "Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMarks_StudentId",
                table: "InternalMarks",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalMarks");
        }
    }
}
