using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseinFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "FileRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Course_Id",
                table: "FileRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_Course_Id",
                table: "FileRecords",
                column: "Course_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Courserecord_Course_Id",
                table: "FileRecords",
                column: "Course_Id",
                principalTable: "Courserecord",
                principalColumn: "Course_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Courserecord_Course_Id",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_Course_Id",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "Course_Id",
                table: "FileRecords");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "FileRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
