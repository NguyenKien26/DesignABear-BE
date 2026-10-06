using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CompleteAccessoryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accessories",
                columns: table => new
                {
                    accessory_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    base_cost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    assembly_cost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    target_price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessories", x => x.accessory_id);
                });

            migrationBuilder.CreateTable(
                name: "product_accessories",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    accessory_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_accessories", x => new { x.product_id, x.accessory_id });
                    table.ForeignKey(
                        name: "FK_product_accessories_accessories_accessory_id",
                        column: x => x.accessory_id,
                        principalTable: "accessories",
                        principalColumn: "accessory_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_accessories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "accessory_id",
                table: "inventory",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_accessory_id",
                table: "inventory",
                column: "accessory_id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_accessories_accessory_id",
                table: "inventory",
                column: "accessory_id",
                principalTable: "accessories",
                principalColumn: "accessory_id");
            
            // Seed data for Smart Chip to support current logic
            migrationBuilder.InsertData(
                table: "accessories",
                columns: new[] { "accessory_id", "name", "sku", "description", "base_cost", "assembly_cost", "target_price", "is_active", "created_at", "updated_at" },
                values: new object[] { Guid.NewGuid(), "Smart Chip", "SMART-CHIP-001", "AI core for intelligent bear", 500000m, 100000m, 999000m, true, DateTime.UtcNow, DateTime.UtcNow });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "product_accessories");
            migrationBuilder.DropTable(name: "accessories");
            migrationBuilder.DropColumn(name: "accessory_id", table: "inventory");
        }
    }
}
