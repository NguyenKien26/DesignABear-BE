using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductTypeFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_type",
                table: "products");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                column: "updated_at",
                value: new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6280));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6122), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6122) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6135), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(6135) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5780), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5782) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5786), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5786) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5788), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5788) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5974), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5975) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5985), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5985) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5988), new DateTime(2026, 4, 21, 1, 48, 23, 27, DateTimeKind.Utc).AddTicks(5988) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_type",
                table: "products",
                type: "character varying(30)",
                maxLength: 30,
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
                columns: new[] { "created_at", "product_type", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2002), "BASE_BEAR", new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2003) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "product_type", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2016), "AI_PROCESSOR", new DateTime(2026, 4, 20, 19, 12, 42, 763, DateTimeKind.Utc).AddTicks(2017) });

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
    }
}
