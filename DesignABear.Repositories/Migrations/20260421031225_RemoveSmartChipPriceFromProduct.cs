using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSmartChipPriceFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "smart_chip_price",
                table: "products");

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(768), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(769) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(983));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(986));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(729), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(730) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(310), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(314) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(318), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(320), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(320) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(554), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(555) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(566), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(567) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(570), new DateTime(2026, 4, 21, 3, 12, 24, 323, DateTimeKind.Utc).AddTicks(570) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "smart_chip_price",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9860), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9861) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 2, 3, 34, 77, DateTimeKind.Utc).AddTicks(327));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 2, 3, 34, 77, DateTimeKind.Utc).AddTicks(332));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9741), 0m, new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9742) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8881), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8884) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8890), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8893), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(8893) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9336), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9337) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9356), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9356) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9361), new DateTime(2026, 4, 21, 2, 3, 34, 76, DateTimeKind.Utc).AddTicks(9361) });
        }
    }
}
