using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SyncAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "method",
                table: "fulfillments");

            migrationBuilder.RenameColumn(
                name: "fulfilled_at",
                table: "fulfillments",
                newName: "shipped_at");

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_delivery_at",
                table: "fulfillments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "carrier",
                table: "fulfillments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "delivered_at",
                table: "fulfillments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "estimated_delivery_at",
                table: "fulfillments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "characters",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "category_id", "name", "parent_id", "slug" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Gấu Nhồi Bông", null, "gau-nhoi-bong" });

            migrationBuilder.InsertData(
                table: "characters",
                columns: new[] { "character_id", "description", "image_url", "is_active", "license_brand", "name", "slug" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Gấu bông cổ điển mềm mại", null, true, null, "Gấu Nâu Classical", "gau-nau-classical" });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "product_id", "created_at", "description", "is_active", "is_personalizable", "model_3d_url", "name", "product_type", "slug", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(5029), "Sản phẩm gấu Teddy cao cấp nhất", true, true, null, "Gấu Teddy Đặc Biệt", "BASE_BEAR", "gau-teddy-dac-biet", new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(5029) });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "role_id", "color_hex", "created_at", "created_by", "description", "icon", "is_active", "is_system", "priority", "role_name", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4783), null, "Software Administrator", null, true, true, 1, "Admin", new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4785), null },
                    { 2, null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4788), null, "Shop Staff", null, true, true, 2, "Staff", new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4788), null },
                    { 3, null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4789), null, "Parent / Customer", null, true, true, 3, "Customer", new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4790), null }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "category_id", "product_id" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                table: "product_characters",
                columns: new[] { "character_id", "product_id" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                table: "product_variants",
                columns: new[] { "variant_id", "currency", "is_sold_out", "price", "product_id", "sku", "variant_name" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), "VND", false, 500000m, new Guid("33333333-3333-3333-3333-333333333333"), "TED-SPEC-001", "Standard" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "avatar_url", "created_at", "date_of_birth", "email", "failed_login_count", "full_name", "gender", "is_locked", "language", "last_login_at", "password_hash", "phone_number", "provider", "provider_id", "refresh_token", "role_id", "status", "timezone", "two_factor_enabled", "updated_at" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4922), null, "admin@designabear.vn", 0, "Admin DesignABear", null, false, "vi", null, "$2a$11$qJ6xaA.cPe0DoGkBFhVXRub5REPJ/60fS1qXra8JYiQcXt4T5hAne", null, "Local", null, null, 1, "Active", "Asia/Ho_Chi_Minh", false, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4922) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4932), null, "staff@designabear.vn", 0, "Staff DesignABear", null, false, "vi", null, "$2a$11$ehYMrDAhwzH07nsnILL3M.2yQgWKRdRxmhDUFlIgdIsmQBMqf.bKq", null, "Local", null, null, 2, "Active", "Asia/Ho_Chi_Minh", false, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4932) },
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4934), null, "user@designabear.vn", 0, "Sample User", null, false, "vi", null, "$2a$11$lcSr4ykln5Di0r5ZnxugROEl0yDBT6m6rd6UJMhf0fA//IBE5Qr7W", null, "Local", null, null, 3, "Active", "Asia/Ho_Chi_Minh", false, new DateTime(2026, 3, 12, 13, 40, 34, 919, DateTimeKind.Utc).AddTicks(4934) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumns: new[] { "category_id", "product_id" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "product_characters",
                keyColumns: new[] { "character_id", "product_id" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "characters",
                keyColumn: "character_id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "actual_delivery_at",
                table: "fulfillments");

            migrationBuilder.DropColumn(
                name: "carrier",
                table: "fulfillments");

            migrationBuilder.DropColumn(
                name: "delivered_at",
                table: "fulfillments");

            migrationBuilder.DropColumn(
                name: "estimated_delivery_at",
                table: "fulfillments");

            migrationBuilder.DropColumn(
                name: "description",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "characters");

            migrationBuilder.RenameColumn(
                name: "shipped_at",
                table: "fulfillments",
                newName: "fulfilled_at");

            migrationBuilder.AddColumn<string>(
                name: "method",
                table: "fulfillments",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
