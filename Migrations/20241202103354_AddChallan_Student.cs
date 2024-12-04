using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddChallan_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Challans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Challans_StudentId",
                table: "Challans",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challans_Studentslist_StudentId",
                table: "Challans",
                column: "StudentId",
                principalTable: "Studentslist",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challans_Studentslist_StudentId",
                table: "Challans");

            migrationBuilder.DropIndex(
                name: "IX_Challans_StudentId",
                table: "Challans");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Challans");
        }
    }
}
