using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddParentProductIdToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "parent_product_id",
                table: "products",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6519));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "parent_product_id", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6307), null, new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6308) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "parent_product_id", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6325), null, new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6325) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5939), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5945), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5946) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5948), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5948) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5950), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5950) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5952), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(5952) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6191), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6191) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6202), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6206), new DateTime(2026, 4, 20, 8, 8, 4, 83, DateTimeKind.Utc).AddTicks(6207) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_product_id",
                table: "products");

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

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4672), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4673) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4674), new DateTime(2026, 4, 20, 3, 32, 55, 668, DateTimeKind.Utc).AddTicks(4674) });

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
    }
}
