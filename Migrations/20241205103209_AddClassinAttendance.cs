using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddClassinAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Attendances");
        }
    }
}
