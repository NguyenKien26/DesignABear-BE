using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueSkuAndFixSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"));

            migrationBuilder.InsertData(
                table: "accessories",
                columns: new[] { "accessory_id", "assembly_cost", "base_cost", "created_at", "description", "image_url", "is_active", "name", "sku", "target_price", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), 20000m, 50000m, new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9860), "Chip xử lý AI cao cấp cho gấu thông minh", null, true, "AI Processor", "CORE-ESP32-AI", 150000m, new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9861) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                columns: new[] { "accessory_id", "product_id", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), null, new DateTime(2026, 4, 21, 2, 3, 34, 77, DateTimeKind.Utc).AddTicks(327) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 2, 3, 34, 77, DateTimeKind.Utc).AddTicks(332));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9741), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9742) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8881), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8884) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8890), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8893), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8893) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9336), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9337) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9356), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9356) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9361), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9361) });

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_sku",
                table: "product_variants",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accessories_sku",
                table: "accessories",
                column: "sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_product_variants_sku",
                table: "product_variants");

            migrationBuilder.DropIndex(
                name: "IX_accessories_sku",
                table: "accessories");

            migrationBuilder.DeleteData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                columns: new[] { "accessory_id", "product_id", "updated_at" },
                values: new object[] { null, new Guid("33333333-3333-3333-3333-333333333334"), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6277) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6280));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6122), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6122) });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "product_id", "created_at", "description", "is_active", "is_personalizable", "model_3d_url", "name", "sku", "slug", "smart_chip_price", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6135), "Chip xử lý AI cao cấp cho gấu thông minh", true, false, null, "AI Processor", "", "core-esp32-ai", 0m, new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6135) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5780), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5782) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5786), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5786) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5788), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5788) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5974), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5975) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5985), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5985) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5988), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5988) });
        }
    }
}
