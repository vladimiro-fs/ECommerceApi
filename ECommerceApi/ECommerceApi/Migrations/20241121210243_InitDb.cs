using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Popular = table.Column<bool>(type: "bit", nullable: false),
                    BestSeller = table.Column<bool>(type: "bit", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    InStock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalValue = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "lanches1.png", "Snacks" },
                    { 2, "combos1.png", "Combos" },
                    { 3, "naturais1.png", "Naturals" },
                    { 4, "refrigerantes1.png", "Drinks" },
                    { 5, "sucos1.png", "Juices" },
                    { 6, "sobremesas1.png", "Desserts" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Available", "BestSeller", "CategoryId", "Details", "ImageUrl", "InStock", "Name", "Popular", "Price" },
                values: new object[,]
                {
                    { 1, true, true, 1, "Plain bread, seasoned beef hamburger, onion, mustard and ketchup.", "hamburger1.jpeg", 13, "Hamburger", true, 15m },
                    { 2, true, false, 1, "Plain bread, seasoned beef hamburger and cheese overall.", "hamburger3.jpeg", 10, "CheeseBurger", true, 18m },
                    { 3, true, false, 1, "Plain bread, seasoned beef hamburger, onion, lettuce, mustard and ketchup.", "hamburger4.jpeg", 13, "CheeseSalad", false, 19m },
                    { 4, false, false, 2, "Plain bread, seasoned beef hamburger, cheese, french fries and soda.", "combo1.jpeg", 10, "Hamburger, french fries, soda", true, 25m },
                    { 5, true, false, 2, "Plain bread, beef hamburger, onion, mayonnaise, ketchup, french fries and soda.", "combo2.jpeg", 13, "CheeseBurger, french fries, soda", false, 27m },
                    { 6, true, false, 2, "Plain bread, beef hamburger, onion, mayonnaise, ketchup, french fries and soda.", "combo3.jpeg", 10, "CheeseSalad, french fries, soda", true, 28m },
                    { 7, true, false, 3, "Whole-corn bread with leaves and tomato.", "lanche_natural1.jpeg", 13, "Natural snack with leaves", false, 14m },
                    { 8, true, false, 3, "Whole-corn bread, leaves, tomato and cheese.", "lanche_natural2.jpeg", 10, "Natural snack with cheese", true, 15m },
                    { 9, true, false, 3, "Vegan snack with healthy ingredients.", "lanche_vegano1.jpeg", 18, "Vegan snack", false, 25m },
                    { 10, true, false, 4, "Coca Cola", "coca_cola1.jpeg", 7, "Coca-Cola", true, 21m },
                    { 11, true, false, 4, "Guaraná", "guarana1.jpeg", 6, "Guaraná", false, 25m },
                    { 12, true, false, 4, "Pepsi", "pepsi1.jpeg", 6, "Pepsi", false, 21m },
                    { 13, true, false, 5, "Nutritious and tasty orange juice.", "suco_laranja.jpeg", 10, "Orange juice", false, 11m },
                    { 14, true, false, 5, "Fresh strawberries juice.", "suco_morango1.jpeg", 13, "Strawberry juice", false, 15m },
                    { 15, true, false, 5, "Natural and sugarless grape juice.", "suco_uva1.jpeg", 10, "Grape juice", false, 13m },
                    { 16, true, false, 4, "Fresh mineral water.", "agua_mineral1.jpeg", 10, "Water", false, 5m },
                    { 17, true, false, 6, "Chocolate cookies with chocolate chips.", "cookie1.jpeg", 10, "Chocolate cookies", true, 8m },
                    { 18, true, true, 6, "Crunchy and tasty vanilla cookies.", "cookie2.jpeg", 13, "Vanilla cookies", false, 8m },
                    { 19, true, false, 6, "Swiss tort with filling.", "torta_suica1.jpeg", 10, "Swiss tort", true, 10m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
