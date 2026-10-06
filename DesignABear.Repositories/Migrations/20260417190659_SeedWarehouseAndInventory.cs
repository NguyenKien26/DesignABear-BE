using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedWarehouseAndInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "location_id", "address_text", "location_type", "name", "phone" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), "123 Đường Sáng Tạo, TP. Thủ Đức, TP. HCM", "WAREHOUSE", "Kho chính DesignABear", "028-1234-5678" });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1048), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1049) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1071), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1072) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(143), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(147) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(156), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(156) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(159), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(160) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(659), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(660) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(680), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(680) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(685), new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(686) });

            migrationBuilder.InsertData(
                table: "inventory",
                columns: new[] { "location_id", "product_id", "on_hand", "reserved", "updated_at" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333"), 50, 0, new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1506) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334"), 999, 0, new DateTime(2026, 4, 17, 19, 6, 57, 580, DateTimeKind.Utc).AddTicks(1503) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") });

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "location_id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6082), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6082) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6097), new DateTime(2026, 4, 17, 18, 59, 53, 607, DateTimeKind.Utc).AddTicks(6097) });

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
        }
    }
}
