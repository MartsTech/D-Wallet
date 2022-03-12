using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class RefreshTokenAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Revoked = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Credit",
                keyColumn: "CreditId",
                keyValue: new Guid("a86f8863-099f-49b2-acec-274476cb559d"),
                column: "TransactionDate",
                value: new DateTime(2022, 3, 12, 17, 26, 33, 508, DateTimeKind.Utc).AddTicks(2695));

            migrationBuilder.UpdateData(
                table: "Debit",
                keyColumn: "DebitId",
                keyValue: new Guid("3b31a10f-a9fe-49ad-94cb-ad32c07d13cb"),
                column: "TransactionDate",
                value: new DateTime(2022, 3, 12, 17, 26, 33, 508, DateTimeKind.Utc).AddTicks(2713));

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.UpdateData(
                table: "Credit",
                keyColumn: "CreditId",
                keyValue: new Guid("a86f8863-099f-49b2-acec-274476cb559d"),
                column: "TransactionDate",
                value: new DateTime(2022, 3, 12, 15, 24, 19, 877, DateTimeKind.Utc).AddTicks(3193));

            migrationBuilder.UpdateData(
                table: "Debit",
                keyColumn: "DebitId",
                keyValue: new Guid("3b31a10f-a9fe-49ad-94cb-ad32c07d13cb"),
                column: "TransactionDate",
                value: new DateTime(2022, 3, 12, 15, 24, 19, 877, DateTimeKind.Utc).AddTicks(3207));
        }
    }
}
