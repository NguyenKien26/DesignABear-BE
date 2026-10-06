using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "is_active",
                value: true);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6371), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6372) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5851), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5855) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5861), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5861) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5865), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(5865) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6118), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6119) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6136), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6136) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6142), new DateTime(2026, 4, 11, 17, 13, 4, 263, DateTimeKind.Utc).AddTicks(6143) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9249), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9249) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9033), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9034) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9037), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9038) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9039), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9039) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9167), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9167) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9178), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9178) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9181), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9181) });
        }
    }
}
