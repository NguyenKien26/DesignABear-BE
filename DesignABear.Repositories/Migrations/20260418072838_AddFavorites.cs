using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7944));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7607), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7608) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7704), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7704) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7214), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7218) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7225), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7225) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7229), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7229) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7461), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7462) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7476), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7476) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7482), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7483) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4767));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4764));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4309), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4334), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4334) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3550), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3556) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3562), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3562) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3564), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(3565) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4045), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4046) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4062), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4062) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4067), new DateTime(2026, 4, 17, 20, 55, 2, 223, DateTimeKind.Utc).AddTicks(4068) });
        }
    }
}
