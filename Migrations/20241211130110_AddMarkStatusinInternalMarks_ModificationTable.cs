using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddMarkStatusinInternalMarks_ModificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarkStatus",
                table: "InternalMarks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkStatus",
                table: "InternalMarks");
        }
    }
}
