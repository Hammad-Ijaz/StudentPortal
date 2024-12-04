using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddInstallmentStudentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Installments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Installments_StudentId",
                table: "Installments",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Studentslist_StudentId",
                table: "Installments",
                column: "StudentId",
                principalTable: "Studentslist",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Studentslist_StudentId",
                table: "Installments");

            migrationBuilder.DropIndex(
                name: "IX_Installments_StudentId",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Installments");
        }
    }
}
