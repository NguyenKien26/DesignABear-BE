using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRefundTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_refunds",
                columns: table => new
                {
                    refund_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_by = table.Column<Guid>(type: "uuid", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    evidence_urls = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    admin_notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_refunds", x => x.refund_id);
                    table.ForeignKey(
                        name: "FK_order_refunds_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_refunds_users_approved_by",
                        column: x => x.approved_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_refunds_users_requested_by",
                        column: x => x.requested_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1656), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1657) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1369), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1373) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1377), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1377) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1379), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1379) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1522), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1523) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1576), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1576) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1580), new DateTime(2026, 3, 18, 3, 21, 2, 160, DateTimeKind.Utc).AddTicks(1580) });

            migrationBuilder.CreateIndex(
                name: "IX_order_refunds_approved_by",
                table: "order_refunds",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_order_refunds_order_id",
                table: "order_refunds",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_refunds_requested_by",
                table: "order_refunds",
                column: "requested_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_refunds");

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(318), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(152), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(155) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(159), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(160), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(160) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(253), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(253) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(263), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(263) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(265), new DateTime(2026, 3, 16, 17, 17, 41, 463, DateTimeKind.Utc).AddTicks(266) });
        }
    }
}
