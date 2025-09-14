using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DBv11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Price = table.Column<decimal>(type: "MONEY", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    InStock = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    Avatar = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Birthdate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerifyCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    ToMail = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    Expired = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValueSql: "NOW() + interval '5 minutes'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifyCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerifyCodes_UserId",
                table: "VerifyCodes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "VerifyCodes");
        }
    }
}
