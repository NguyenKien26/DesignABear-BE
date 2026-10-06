using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MigrateToProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "parent_product_id",
                table: "products");


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

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    size_tag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    size_description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    base_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.Sql(@"
                INSERT INTO product_variants (variant_id, product_id, size_tag, size_description, price, base_cost, sku, is_active, created_at, updated_at)
                SELECT 
                    product_id, 
                    product_id, 
                    COALESCE(size_tag, 'OS'), 
                    COALESCE(size, 'Standard Size'), 
                    price, 
                    0, 
                    sku, 
                    is_active, 
                    created_at, 
                    updated_at
                FROM products;
            ");

            migrationBuilder.DropColumn(
                name: "price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "size",
                table: "products");

            migrationBuilder.DropColumn(
                name: "size_tag",
                table: "products");

            migrationBuilder.DropColumn(
                name: "sku",
                table: "products");

            migrationBuilder.InsertData(
                table: "product_variants",
                columns: new[] { "variant_id", "base_cost", "created_at", "is_active", "price", "product_id", "size_description", "size_tag", "sku", "updated_at" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), 250000m, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6121), true, 500000m, new Guid("33333333-3333-3333-3333-333333333333"), "45cm Ngồi", "M", "TED-SPEC-001-M", new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6121) },
                    { new Guid("44444444-4444-4444-4444-444444444442"), 350000m, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6129), true, 650000m, new Guid("33333333-3333-3333-3333-333333333333"), "60cm Siêu Bự", "L", "TED-SPEC-001-L", new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6130) },
                    { new Guid("44444444-4444-4444-4444-444444444443"), 500000m, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6133), true, 999000m, new Guid("33333333-3333-3333-3333-333333333334"), "Free Size", "OS", "ESP32-AI-001-OS", new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6133) }
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6079), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6080) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6089), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6089) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5791), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5798), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5798) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5800), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5802), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5804), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5976), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5977) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5987), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5988) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5991), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5992) });

            migrationBuilder.InsertData(
                table: "inventory",
                columns: new[] { "location_id", "variant_id", "on_hand", "reserved", "updated_at" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441"), 50, 0, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6349) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442"), 50, 0, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6351) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443"), 999, 0, new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6347) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_items_product_variants_variant_id",
                table: "cart_items");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_product_variants_variant_id",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_product_variants_variant_id",
                table: "order_items");

            migrationBuilder.DropTable(
                name: "product_variants");


            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "order_items",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_variant_id",
                table: "order_items",
                newName: "IX_order_items_product_id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "inventory",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_variant_id",
                table: "inventory",
                newName: "IX_inventory_product_id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "cart_items",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_items_variant_id",
                table: "cart_items",
                newName: "IX_cart_items_product_id");

            migrationBuilder.AddColumn<Guid>(
                name: "parent_product_id",
                table: "products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "products",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "size_tag",
                table: "products",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "products",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "inventory",
                columns: new[] { "location_id", "product_id", "on_hand", "reserved", "updated_at" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333"), 50, 0, new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6521) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334"), 999, 0, new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6519) }
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "parent_product_id", "price", "size", "size_tag", "sku", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6307), null, 500000m, null, null, "TED-SPEC-001", new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6308) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "parent_product_id", "price", "size", "size_tag", "sku", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6325), null, 999000m, null, null, "ESP32-AI-001", new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6325) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5939), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5945), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5946) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5948), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5948) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5950), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5950) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5952), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5952) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6191), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6191) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6202), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6206), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6207) });

            migrationBuilder.CreateIndex(
                name: "IX_products_price",
                table: "products",
                column: "price");

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
    }
}
