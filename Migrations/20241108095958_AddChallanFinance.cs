using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiValidation.Migrations
{
    /// <inheritdoc />
    public partial class AddChallanFinance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challans",
                columns: table => new
                {
                    ChallanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challans", x => x.ChallanId);
                    table.ForeignKey(
                        name: "FK_Challans_Studentslist_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studentslist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinanceDetailss",
                columns: table => new
                {
                    FinanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Session = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Installments = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceDetailss", x => x.FinanceId);
                });

            migrationBuilder.CreateTable(
                name: "ChallanFinanceDetails",
                columns: table => new
                {
                    ChallanFinanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallanId = table.Column<int>(type: "int", nullable: false),
                    FinanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallanFinanceDetails", x => x.ChallanFinanceId);
                    table.ForeignKey(
                        name: "FK_ChallanFinanceDetails_Challans_ChallanId",
                        column: x => x.ChallanId,
                        principalTable: "Challans",
                        principalColumn: "ChallanId");
                    table.ForeignKey(
                        name: "FK_ChallanFinanceDetails_FinanceDetailss_FinanceId",
                        column: x => x.FinanceId,
                        principalTable: "FinanceDetailss",
                        principalColumn: "FinanceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallanFinanceDetails_ChallanId",
                table: "ChallanFinanceDetails",
                column: "ChallanId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallanFinanceDetails_FinanceId",
                table: "ChallanFinanceDetails",
                column: "FinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Challans_StudentId",
                table: "Challans",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallanFinanceDetails");

            migrationBuilder.DropTable(
                name: "Challans");

            migrationBuilder.DropTable(
                name: "FinanceDetailss");
        }
    }
}
