using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UnifyInventoryIdentityPureFinalV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_accessories_accessory_id",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_product_variants_variant_id",
                table: "inventory");

            migrationBuilder.DropIndex(
                name: "IX_inventory_accessory_id",
                table: "inventory");

            migrationBuilder.DropIndex(
                name: "IX_inventory_variant_id",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "accessory_id",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "variant_id",
                table: "inventory");

            migrationBuilder.AddColumn<Guid>(
                name: "identity_id",
                table: "inventory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "is_accessory",
                table: "inventory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8080), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8081) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                columns: new[] { "identity_id", "is_accessory", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), true, new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8398) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                columns: new[] { "identity_id", "is_accessory", "updated_at" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), false, new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8409) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8015), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(8016) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7500), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7505) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7511), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7512) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7515), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7516) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7848), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7849) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7871), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7871) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7877), new DateTime(2026, 4, 21, 20, 23, 22, 991, DateTimeKind.Utc).AddTicks(7878) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "identity_id",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "is_accessory",
                table: "inventory");

            migrationBuilder.AddColumn<Guid>(
                name: "accessory_id",
                table: "inventory",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "variant_id",
                table: "inventory",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "accessories",
                keyColumn: "accessory_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4991), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4991) });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000001"),
                columns: new[] { "accessory_id", "updated_at", "variant_id" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333334"), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(5147), null });

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumn: "inventory_id",
                keyValue: new Guid("cccc0000-0000-0000-0000-000000000002"),
                columns: new[] { "accessory_id", "updated_at", "variant_id" },
                values: new object[] { null, new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(5150), null });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4952), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4953) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4627), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4629) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4633), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4633) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4635), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4635) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4820), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4821) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4832), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4832) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4835), new DateTime(2026, 4, 21, 17, 18, 7, 525, DateTimeKind.Utc).AddTicks(4836) });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_accessory_id",
                table: "inventory",
                column: "accessory_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_variant_id",
                table: "inventory",
                column: "variant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_accessories_accessory_id",
                table: "inventory",
                column: "accessory_id",
                principalTable: "accessories",
                principalColumn: "accessory_id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_product_variants_variant_id",
                table: "inventory",
                column: "variant_id",
                principalTable: "product_variants",
                principalColumn: "variant_id");
        }
    }
}
