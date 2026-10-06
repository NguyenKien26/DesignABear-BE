using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(5186));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(5184));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4961), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4962) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4978), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4978) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4660), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4662) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4668), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4668) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4670), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4671) });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "role_id", "color_hex", "created_at", "created_by", "description", "icon", "is_active", "is_system", "priority", "role_name", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 4, null, new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4672), null, "Product Artisan / Bear Maker", null, true, true, 4, "Craftsman", new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4673), null },
                    { 5, null, new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4674), null, "QC Inspector", null, true, true, 5, "QualityControl", new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4674), null }
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4861), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4861) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4873), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4873) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4876), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4877) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(842));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(839));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9875), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9876) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(172), new DateTime(2026, 4, 20, 3, 27, 1, 665, DateTimeKind.Utc).AddTicks(175) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9095), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9100) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9107), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9107) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9110), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9111) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9575), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9576) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9595), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9595) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9599), new DateTime(2026, 4, 20, 3, 27, 1, 664, DateTimeKind.Utc).AddTicks(9600) });
        }
    }
}
