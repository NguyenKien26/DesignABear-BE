using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CreateFavoriteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    favorite_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.favorite_id);
                    table.ForeignKey(
                        name: "FK_favorites_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorites_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_favorites_product_id",
                table: "favorites",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_user_product",
                table: "favorites",
                columns: new[] { "user_id", "product_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7944));

            migrationBuilder.UpdateData(
                table: "inventory",
                keyColumns: new[] { "location_id", "product_id" },
                keyValues: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("33333333-3333-3333-3333-333333333334") },
                column: "updated_at",
                value: new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7607), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7608) });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7704), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7704) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7214), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7218) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7225), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7225) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7229), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7229) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7461), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7462) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7476), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7476) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7482), new DateTime(2026, 4, 18, 7, 28, 34, 399, DateTimeKind.Utc).AddTicks(7483) });
        }
    }
}
