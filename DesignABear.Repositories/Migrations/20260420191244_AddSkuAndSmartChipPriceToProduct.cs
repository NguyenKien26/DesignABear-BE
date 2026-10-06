using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSkuAndSmartChipPriceToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "products",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");


            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2370));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2373));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "sku", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2002), "", 0m, new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2003) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "sku", "smart_chip_price", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2016), "", 0m, new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2017) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1493), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1496) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1502), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1503) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1504), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1505) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1822), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1823) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1857), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1858) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1861), new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(1861) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sku",
                table: "products");


            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(1034));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(1037));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(903), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(903) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(915), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(915) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(679), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(682) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(685), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(685) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(687), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(687) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(803), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(803) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(840), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(841) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(843), new DateTime(2026, 4, 20, 18, 3, 53, 149, DateTimeKind.Utc).AddTicks(844) });
        }
    }
}
