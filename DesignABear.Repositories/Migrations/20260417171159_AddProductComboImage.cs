using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddProductComboImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_combo_images",
                columns: table => new
                {
                    combo_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    base_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    combination_key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    image_url = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_combo_images", x => x.combo_id);
                    table.ForeignKey(
                        name: "FK_product_combo_images_products_base_product_id",
                        column: x => x.base_product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(5001), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(5002) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4756), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4759) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4762), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4762) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4763), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4764) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4870), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4871) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4882), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4882) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4885), new DateTime(2026, 4, 17, 17, 11, 58, 649, DateTimeKind.Utc).AddTicks(4885) });

            migrationBuilder.CreateIndex(
                name: "IX_product_combo_images_base_key",
                table: "product_combo_images",
                columns: new[] { "base_product_id", "combination_key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_combo_images");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7741), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7742) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7490), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7492) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7496), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7496) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7498), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7499) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7639), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7639) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7650), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7651) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7654), new DateTime(2026, 4, 17, 8, 5, 19, 886, DateTimeKind.Utc).AddTicks(7654) });
        }
    }
}
