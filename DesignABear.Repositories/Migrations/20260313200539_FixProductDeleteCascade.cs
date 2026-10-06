using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixProductDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalization_rules_products_addon_product_id",
                table: "personalization_rules");

            migrationBuilder.DropForeignKey(
                name: "FK_personalization_rules_products_base_product_id",
                table: "personalization_rules");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5715), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5717) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3495), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3502) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3627), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3628) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3632), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(3633) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(4875), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(4876) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5062), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5063) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5073), new DateTime(2026, 3, 13, 20, 5, 35, 604, DateTimeKind.Utc).AddTicks(5074) });

            migrationBuilder.AddForeignKey(
                name: "FK_personalization_rules_products_addon_product_id",
                table: "personalization_rules",
                column: "addon_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_personalization_rules_products_base_product_id",
                table: "personalization_rules",
                column: "base_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalization_rules_products_addon_product_id",
                table: "personalization_rules");

            migrationBuilder.DropForeignKey(
                name: "FK_personalization_rules_products_base_product_id",
                table: "personalization_rules");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(999), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(999) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(757), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(761) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(764), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(765) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(766), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(766) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(925), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(925) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(935), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(935) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(938), new DateTime(2026, 3, 12, 18, 24, 8, 358, DateTimeKind.Utc).AddTicks(938) });

            migrationBuilder.AddForeignKey(
                name: "FK_personalization_rules_products_addon_product_id",
                table: "personalization_rules",
                column: "addon_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_personalization_rules_products_base_product_id",
                table: "personalization_rules",
                column: "base_product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
