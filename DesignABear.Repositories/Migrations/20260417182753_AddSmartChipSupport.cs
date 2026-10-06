using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartChipSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "smart_chip_price",
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

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7188), 999000m, new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7189) });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "product_id", "created_at", "description", "is_active", "is_personalizable", "min_price", "model_3d_url", "name", "product_type", "slug", "smart_chip_price", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7202), "Chip xử lý AI cao cấp cho gấu thông minh", true, false, 999000m, null, "ESP32 AI Smart Chip", "SMART_CHIP", "core-esp32-ai", 0m, new DateTime(2026, 4, 17, 18, 27, 52, 433, DateTimeKind.Utc).AddTicks(7203) });

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

            migrationBuilder.InsertData(
                table: "product_variants",
                columns: new[] { "variant_id", "currency", "image_url", "is_sold_out", "price", "product_id", "sku", "variant_name", "weight_gram" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444445"), "VND", null, false, 999000m, new Guid("33333333-3333-3333-3333-333333333334"), "ESP32-AI-001", "Standard", 500f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444445"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"));

            migrationBuilder.DropColumn(
                name: "smart_chip_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "cart_items");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(5001), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(5002) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4756), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4759) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4762), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4762) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4763), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4764) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4870), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4871) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4882), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4882) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4885), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4885) });
        }
    }
}
