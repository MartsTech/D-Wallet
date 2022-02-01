using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExternalUserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currency = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    CreditId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Currency = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.CreditId);
                    table.ForeignKey(
                        name: "FK_Credit_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Debit",
                columns: table => new
                {
                    DebitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Currency = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debit", x => x.DebitId);
                    table.ForeignKey(
                        name: "FK_Debit_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "Currency", "ExternalUserId" },
                values: new object[] { new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"), "USD", "df1525ce-1f1b-4e22-81fd-1065a35e4d5c" });

            migrationBuilder.InsertData(
                table: "Credit",
                columns: new[] { "CreditId", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("a86f8863-099f-49b2-acec-274476cb559d"), new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"), "USD", new DateTime(2022, 2, 1, 14, 2, 18, 751, DateTimeKind.Utc).AddTicks(8011), 400m });

            migrationBuilder.InsertData(
                table: "Debit",
                columns: new[] { "DebitId", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("3b31a10f-a9fe-49ad-94cb-ad32c07d13cb"), new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"), "USD", new DateTime(2022, 2, 1, 14, 2, 18, 751, DateTimeKind.Utc).AddTicks(8029), 50m });

            migrationBuilder.CreateIndex(
                name: "IX_Credit_AccountId",
                table: "Credit",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Debit_AccountId",
                table: "Debit",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "Debit");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
