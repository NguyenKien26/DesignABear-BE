using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAndCostFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "assembly_cost",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "base_cost",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "smart_chip_price",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "has_smart_chip",
                table: "production_jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_defect_verified",
                table: "order_refunds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<bool>(
                name: "includes_smart_chip",
                table: "builds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "assembly_cost", "base_cost", "created_at", "smart_chip_price", "updated_at" },
                values: new object[] { 0m, 0m, new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5892), 0m, new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5893) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "assembly_cost", "base_cost", "created_at", "smart_chip_price", "updated_at" },
                values: new object[] { 0m, 0m, new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5944), 0m, new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5945) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5207), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5213) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5221), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5221) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5225), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5226) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5709), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5710) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5723), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5724) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5727), new DateTime(2026, 4, 18, 15, 57, 48, 337, DateTimeKind.Utc).AddTicks(5727) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "assembly_cost",
                table: "products");

            migrationBuilder.DropColumn(
                name: "base_cost",
                table: "products");

            migrationBuilder.DropColumn(
                name: "smart_chip_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "has_smart_chip",
                table: "production_jobs");

            migrationBuilder.DropColumn(
                name: "is_defect_verified",
                table: "order_refunds");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "includes_smart_chip",
                table: "builds");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9398));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9395));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9070), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9071) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9136), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(9136) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8742), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8746) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8750), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8750) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8752), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8753) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8960), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8960) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8977), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8977) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8980), new DateTime(2026, 4, 18, 7, 33, 44, 887, DateTimeKind.Utc).AddTicks(8981) });
        }
    }
}
