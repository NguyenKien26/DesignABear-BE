using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightToProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "weight_gram",
                table: "product_variants",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "weight_gram",
                value: 500f);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7464), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7464) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7262), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7265) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7268), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7269) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7271), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7271) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7391), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7392) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7403), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7403) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7405), new DateTime(2026, 4, 13, 16, 37, 8, 475, DateTimeKind.Utc).AddTicks(7405) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight_gram",
                table: "product_variants");

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
    }
}
