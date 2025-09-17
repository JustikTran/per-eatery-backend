using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DBv12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    RecevierId = table.Column<Guid>(type: "UUID", nullable: false),
                    Message = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecevierId",
                        column: x => x.RecevierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    IsActive = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    DishId = table.Column<Guid>(type: "UUID", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "UUID", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "UUID", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    OrderId = table.Column<Guid>(type: "UUID", nullable: false),
                    DishId = table.Column<Guid>(type: "UUID", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "MONEY", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_DishId",
                table: "CartItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecevierId",
                table: "Messages",
                column: "RecevierId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DishId",
                table: "OrderItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentMethods");
        }
    }
}
