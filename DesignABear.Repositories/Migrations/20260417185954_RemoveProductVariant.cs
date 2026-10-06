using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_build_components_product_variants_option_variant_id",
                table: "build_components");

            migrationBuilder.DropForeignKey(
                name: "FK_builds_product_variants_base_variant_id",
                table: "builds");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_items_product_variants_variant_id",
                table: "cart_items");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_product_variants_variant_id",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_product_variants_variant_id",
                table: "order_items");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "order_items",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "inventory",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "cart_items",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "base_variant_id",
                table: "builds",
                newName: "base_product_id");

            migrationBuilder.RenameColumn(
                name: "option_variant_id",
                table: "build_components",
                newName: "option_product_id");

            // --- DATA MIGRATION START ---
            // Update order_items with product_id from variants
            migrationBuilder.Sql("UPDATE order_items SET product_id = pv.product_id FROM product_variants pv WHERE order_items.product_id = pv.variant_id");
            
            // Update inventory with product_id from variants
            migrationBuilder.Sql("UPDATE inventory SET product_id = pv.product_id FROM product_variants pv WHERE inventory.product_id = pv.variant_id");
            
            // Update cart_items with product_id from variants
            migrationBuilder.Sql("UPDATE cart_items SET product_id = pv.product_id FROM product_variants pv WHERE cart_items.product_id = pv.variant_id");
            
            // Update builds with base_product_id from variants
            migrationBuilder.Sql("UPDATE builds SET base_product_id = pv.product_id FROM product_variants pv WHERE builds.base_product_id = pv.variant_id");
            
            // Update build_components with option_product_id from variants
            migrationBuilder.Sql("UPDATE build_components SET option_product_id = pv.product_id FROM product_variants pv WHERE build_components.option_product_id = pv.variant_id");
            // --- DATA MIGRATION END ---

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropIndex(
                name: "IX_products_min_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "min_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "cart_items");

            migrationBuilder.RenameColumn(
                name: "smart_chip_price",
                table: "products",
                newName: "price");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_variant_id",
                table: "order_items",
                newName: "IX_order_items_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_variant_id",
                table: "inventory",
                newName: "IX_inventory_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_items_variant_id",
                table: "cart_items",
                newName: "IX_cart_items_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_builds_base_variant_id",
                table: "builds",
                newName: "IX_builds_base_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_build_components_option_variant_id",
                table: "build_components",
                newName: "IX_build_components_option_product_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_sold_out",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "products",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "weight_gram",
                table: "products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "is_sold_out", "price", "sku", "updated_at", "weight_gram" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6082), false, 500000m, "TED-SPEC-001", new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6082), 500f });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "is_sold_out", "name", "price", "product_type", "sku", "updated_at", "weight_gram" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6097), false, "AI Processor", 999000m, "AI_PROCESSOR", "ESP32-AI-001", new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6097), 10f });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5863), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5866) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5869), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5869) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5870), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(5871) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6006), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6006) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6015), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6015) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6018), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6018) });

            migrationBuilder.CreateIndex(
                name: "IX_products_price",
                table: "products",
                column: "price");

            migrationBuilder.AddForeignKey(
                name: "FK_build_components_products_option_product_id",
                table: "build_components",
                column: "option_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_builds_products_base_product_id",
                table: "builds",
                column: "base_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_items_products_product_id",
                table: "cart_items",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_products_product_id",
                table: "inventory",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_products_product_id",
                table: "order_items",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_build_components_products_option_product_id",
                table: "build_components");

            migrationBuilder.DropForeignKey(
                name: "FK_builds_products_base_product_id",
                table: "builds");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_items_products_product_id",
                table: "cart_items");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_products_product_id",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_products_product_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_products_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "is_sold_out",
                table: "products");

            migrationBuilder.DropColumn(
                name: "sku",
                table: "products");

            migrationBuilder.DropColumn(
                name: "weight_gram",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "products",
                newName: "smart_chip_price");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "order_items",
                newName: "variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_product_id",
                table: "order_items",
                newName: "IX_order_items_variant_id");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "inventory",
                newName: "variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_product_id",
                table: "inventory",
                newName: "IX_inventory_variant_id");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "cart_items",
                newName: "variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_items_product_id",
                table: "cart_items",
                newName: "IX_cart_items_variant_id");

            migrationBuilder.RenameColumn(
                name: "base_product_id",
                table: "builds",
                newName: "base_variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_builds_base_product_id",
                table: "builds",
                newName: "IX_builds_base_variant_id");

            migrationBuilder.RenameColumn(
                name: "option_product_id",
                table: "build_components",
                newName: "option_variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_build_components_option_product_id",
                table: "build_components",
                newName: "IX_build_components_option_variant_id");

            migrationBuilder.AddColumn<decimal>(
                name: "min_price",
                table: "products",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "includes_smart_chip",
                table: "order_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "includes_smart_chip",
                table: "cart_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    image_url = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    is_sold_out = table.Column<bool>(type: "boolean", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    variant_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    weight_gram = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.variant_id);
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "product_variants",
                columns: new[] { "variant_id", "currency", "image_url", "is_sold_out", "price", "product_id", "sku", "variant_name", "weight_gram" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "VND", null, false, 500000m, new Guid("33333333-3333-3333-3333-333333333333"), "TED-SPEC-001", "Standard", 500f },
                    { new Guid("44444444-4444-4444-4444-444444444445"), "VND", null, false, 999000m, new Guid("33333333-3333-3333-3333-333333333334"), "ESP32-AI-001", "Standard", 500f }
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "min_price", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7188), 500000m, 999000m, new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7189) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "min_price", "name", "product_type", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7202), 999000m, "ESP32 AI Smart Chip", "SMART_CHIP", 0m, new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7203) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6610) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6614), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6614) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6616), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(6616) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7012), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7014) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7026), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7026) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7030), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7030) });

            migrationBuilder.CreateIndex(
                name: "IX_products_min_price",
                table: "products",
                column: "min_price");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_build_components_product_variants_option_variant_id",
                table: "build_components",
                column: "option_variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_builds_product_variants_base_variant_id",
                table: "builds",
                column: "base_variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_items_product_variants_variant_id",
                table: "cart_items",
                column: "variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_product_variants_variant_id",
                table: "inventory",
                column: "variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_product_variants_variant_id",
                table: "order_items",
                column: "variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
