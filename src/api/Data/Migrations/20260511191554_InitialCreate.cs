using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaunchCart.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enquiries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "ImageUrl", "IsActive", "Name", "Price", "Slug" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 5, 11, 19, 15, 54, 463, DateTimeKind.Unspecified).AddTicks(3542), new TimeSpan(0, 0, 0, 0, 0)), "Handcrafted full-grain leather tote, ideal for everyday use.", "/images/leather-tote.jpg", true, "Leather Tote Bag", 149.99m, "leather-tote-bag" },
                    { 2, new DateTimeOffset(new DateTime(2026, 5, 11, 19, 15, 54, 463, DateTimeKind.Unspecified).AddTicks(3563), new TimeSpan(0, 0, 0, 0, 0)), "Durable waxed-canvas backpack with laptop compartment.", "/images/canvas-backpack.jpg", true, "Canvas Backpack", 89.99m, "canvas-backpack" },
                    { 3, new DateTimeOffset(new DateTime(2026, 5, 11, 19, 15, 54, 463, DateTimeKind.Unspecified).AddTicks(3566), new TimeSpan(0, 0, 0, 0, 0)), "Soft merino wool throw in a classic herringbone weave.", "/images/wool-throw.jpg", true, "Wool Throw Blanket", 119.99m, "wool-throw-blanket" },
                    { 4, new DateTimeOffset(new DateTime(2026, 5, 11, 19, 15, 54, 463, DateTimeKind.Unspecified).AddTicks(3568), new TimeSpan(0, 0, 0, 0, 0)), "Hand-thrown stoneware mug, microwave and dishwasher safe.", "/images/ceramic-mug.jpg", true, "Ceramic Coffee Mug", 34.99m, "ceramic-coffee-mug" },
                    { 5, new DateTimeOffset(new DateTime(2026, 5, 11, 19, 15, 54, 463, DateTimeKind.Unspecified).AddTicks(3571), new TimeSpan(0, 0, 0, 0, 0)), "Set of three hand-poured beeswax candles with cotton wicks.", "/images/beeswax-candles.jpg", true, "Beeswax Candle Set", 44.99m, "beeswax-candle-set" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_ProductId",
                table: "Enquiries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
