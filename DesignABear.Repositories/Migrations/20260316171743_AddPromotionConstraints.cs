using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "promotions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "max_usage_count",
                table: "promotions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "max_usage_per_user",
                table: "promotions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "min_order_amount",
                table: "promotions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "usage_count",
                table: "promotions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(318), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(152), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(155) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(159), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(160), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(160) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(253), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(253) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(263), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(263) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(265), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(266) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "promotions");

            migrationBuilder.DropColumn(
                name: "max_usage_count",
                table: "promotions");

            migrationBuilder.DropColumn(
                name: "max_usage_per_user",
                table: "promotions");

            migrationBuilder.DropColumn(
                name: "min_order_amount",
                table: "promotions");

            migrationBuilder.DropColumn(
                name: "usage_count",
                table: "promotions");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6580), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6581) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5838), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5845) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5856), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5856) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5861), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(5862) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6219), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6221) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6242), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6243) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6252), new DateTime(2026, 3, 16, 6, 25, 58, 881, DateTimeKind.Utc).AddTicks(6253) });
        }
    }
}
