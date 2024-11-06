using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIdtoStudentId_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Studentslist_StudentrecId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StudentrecId",
                table: "Users",
                newName: "StudentrecStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_StudentrecId",
                table: "Users",
                newName: "IX_Users_StudentrecStudentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Studentslist",
                newName: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Studentslist_StudentrecStudentId",
                table: "Users",
                column: "StudentrecStudentId",
                principalTable: "Studentslist",
                principalColumn: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Studentslist_StudentrecStudentId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StudentrecStudentId",
                table: "Users",
                newName: "StudentrecId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_StudentrecStudentId",
                table: "Users",
                newName: "IX_Users_StudentrecId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Studentslist",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Studentslist_StudentrecId",
                table: "Users",
                column: "StudentrecId",
                principalTable: "Studentslist",
                principalColumn: "Id");
        }
    }
}
