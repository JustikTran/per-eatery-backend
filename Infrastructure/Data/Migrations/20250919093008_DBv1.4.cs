using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DBv14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressReceives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    Receiver = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Phone = table.Column<string>(type: "VARCHAR(15)", nullable: false),
                    Address = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressReceives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressReceives_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressReceives_UserId",
                table: "AddressReceives",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressReceives");
        }
    }
}
