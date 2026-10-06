using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddAssemblyCostToVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "assembly_cost",
                table: "product_variants",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

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
                columns: new[] { "assembly_cost", "created_at", "updated_at" },
                values: new object[] { 0m, new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(628), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(629) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "assembly_cost", "created_at", "updated_at" },
                values: new object[] { 0m, new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(643), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(643) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "assembly_cost", "created_at", "updated_at" },
                values: new object[] { 0m, new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(854), new DateTime(2026, 4, 20, 10, 6, 31, 81, DateTimeKind.Utc).AddTicks(854) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "assembly_cost",
                table: "product_variants");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444441") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6349));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444442") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6351));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "variant_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444443") },
                column: "updated_at",
                value: new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6347));

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6121), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6121) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6129), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6130) });

            migrationBuilder.UpdateData(
                table: "product_variants",
                keyColumn: "variant_id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6133), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6133) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6079), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6080) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6089), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(6089) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5791), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5798), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5798) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5800), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5802), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5804), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5976), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5977) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5987), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5988) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5991), new DateTime(2026, 4, 20, 9, 41, 47, 265, DateTimeKind.Utc).AddTicks(5992) });
        }
    }
}
