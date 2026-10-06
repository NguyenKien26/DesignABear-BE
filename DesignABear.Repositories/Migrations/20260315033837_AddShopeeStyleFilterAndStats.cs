using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddShopeeStyleFilterAndStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "min_price",
                table: "products",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "total_sales",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "product_dynamic_stats",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    view_count = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    discount_rate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_dynamic_stats", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_product_dynamic_stats_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "product_dynamic_stats",
                columns: new[] { "product_id", "discount_rate" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), 0.0 });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "min_price", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7611), 500000m, new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7611) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7430), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7432) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7437), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7439), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7439) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7540), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7550), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7550) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7553), new DateTime(2026, 3, 15, 3, 38, 35, 51, DateTimeKind.Utc).AddTicks(7553) });

            migrationBuilder.CreateIndex(
                name: "IX_products_min_price",
                table: "products",
                column: "min_price");

            migrationBuilder.CreateIndex(
                name: "IX_products_total_sales",
                table: "products",
                column: "total_sales");

            migrationBuilder.CreateIndex(
                name: "IX_stats_view_count",
                table: "product_dynamic_stats",
                column: "view_count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_dynamic_stats");

            migrationBuilder.DropIndex(
                name: "IX_products_min_price",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_total_sales",
                table: "products");

            migrationBuilder.DropColumn(
                name: "min_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "total_sales",
                table: "products");

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
        }
    }
}
