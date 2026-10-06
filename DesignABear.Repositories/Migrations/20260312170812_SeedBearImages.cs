using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedBearImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "product_media",
                columns: new[] { "media_id", "alt_text", "product_id", "sort_order", "url" },
                values: new object[,]
                {
                    { new Guid("aaaa0001-0001-0001-0001-000000000001"), "Gấu Teddy Đặc Biệt - Mặt trước", new Guid("33333333-3333-3333-3333-333333333333"), 1, "https://images.unsplash.com/photo-1559715541-5daf8a0296d0?w=600" },
                    { new Guid("aaaa0001-0001-0001-0001-000000000002"), "Gấu Teddy Đặc Biệt - Góc nghiêng", new Guid("33333333-3333-3333-3333-333333333333"), 2, "https://images.unsplash.com/photo-1602734846297-9299fc2d4f98?w=600" },
                    { new Guid("aaaa0001-0001-0001-0001-000000000003"), "Gấu Teddy Đặc Biệt - Chi tiết vải", new Guid("33333333-3333-3333-3333-333333333333"), 3, "https://images.unsplash.com/photo-1562040506-a9b32cb51b94?w=600" }
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9995), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9995) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9764), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9766) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9769), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9769) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9770), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9771) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9888), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9888) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9898), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9899) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9901), new DateTime(2026, 3, 12, 17, 8, 11, 811, DateTimeKind.Utc).AddTicks(9902) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product_media",
                keyColumn: "media_id",
                keyValue: new Guid("aaaa0001-0001-0001-0001-000000000001"));

            migrationBuilder.DeleteData(
                table: "product_media",
                keyColumn: "media_id",
                keyValue: new Guid("aaaa0001-0001-0001-0001-000000000002"));

            migrationBuilder.DeleteData(
                table: "product_media",
                keyColumn: "media_id",
                keyValue: new Guid("aaaa0001-0001-0001-0001-000000000003"));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3181), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3181) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2938), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2940) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2943), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2943) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2945), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(2945) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3066), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3066) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3115), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3115) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3118), new DateTime(2026, 3, 12, 14, 15, 55, 887, DateTimeKind.Utc).AddTicks(3118) });
        }
    }
}
