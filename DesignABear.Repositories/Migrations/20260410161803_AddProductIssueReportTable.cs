using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIssueReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_issue_reports",
                columns: table => new
                {
                    report_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_staff_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    resolution = table.Column<string>(type: "text", nullable: true),
                    request_refund = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    staff_notes = table.Column<string>(type: "text", nullable: true),
                    repair_notes = table.Column<string>(type: "text", nullable: true),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    evidence_urls = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_issue_reports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_product_issue_reports_order_items_order_item_id",
                        column: x => x.order_item_id,
                        principalTable: "order_items",
                        principalColumn: "order_item_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_issue_reports_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_issue_reports_users_assigned_staff_id",
                        column: x => x.assigned_staff_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_issue_reports_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "product_id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9249), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9249) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9033), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9034) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9037), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9038) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9039), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9039) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9167), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9167) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9178), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9178) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9181), new DateTime(2026, 4, 10, 16, 18, 1, 168, DateTimeKind.Utc).AddTicks(9181) });

            migrationBuilder.CreateIndex(
                name: "IX_product_issue_reports_assigned_staff_id",
                table: "product_issue_reports",
                column: "assigned_staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_issue_reports_order_item_id",
                table: "product_issue_reports",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_issue_reports_product_id",
                table: "product_issue_reports",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_issue_reports_user_id",
                table: "product_issue_reports",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_issue_reports");

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
        }
    }
}
