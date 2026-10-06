using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MoveWeightAndFixPricingMap : Migration
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

            migrationBuilder.Sql("UPDATE product_variants pv SET weight_gram = p.weight_gram FROM products p WHERE pv.product_id = p.product_id");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5565));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                columns: new[] { "created_at", "updated_at", "weight_gram" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5279), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5280), 500f });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "created_at", "updated_at", "weight_gram" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5295), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5296), 500f });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "created_at", "updated_at", "weight_gram" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5302), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(5303), 500f });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4916), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4917) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4934), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4934) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4252), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4256) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4262), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4263) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4267), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4267) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4270), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4271) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4274), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4275) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4619), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4620) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4638), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4638) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4645), new DateTime(2026, 4, 20, 10, 18, 27, 458, DateTimeKind.Utc).AddTicks(4645) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight_gram",
                table: "product_variants");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(1656));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(1658));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(1631));

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(628), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(629) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(643), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(643) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(854), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(854) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(532), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(533) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(546), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(546) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9662), new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9668) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9674), new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9675) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9677), new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9677) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9679), new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9680) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9681), new DateTime(2026, 4, 20, 10, 6, 31, 80, DateTimeKind.Utc).AddTicks(9682) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(265), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(268) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(286), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(286) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(291), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(292) });
        }
    }
}
