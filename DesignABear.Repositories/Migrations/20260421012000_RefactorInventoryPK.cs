using DesignABear.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260421012000_RefactorInventoryPK")]
    public partial class RefactorInventoryPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop existing composite PK (location_id, variant_id)
            migrationBuilder.DropPrimaryKey(
                name: "PK_inventory",
                table: "inventory");

            // 2. Add inventory_id column as the new Primary Key
            migrationBuilder.AddColumn<Guid>(
                name: "inventory_id",
                table: "inventory",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            // 3. Add product_id column (it was renamed to variant_id in a previous migration)
            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                table: "inventory",
                type: "uuid",
                nullable: true);

            // 4. Set NEW Primary Key
            migrationBuilder.AddPrimaryKey(
                name: "PK_inventory",
                table: "inventory",
                column: "inventory_id");

            // 5. Setup Foreign Keys
            migrationBuilder.AddForeignKey(
                name: "FK_inventory_products_product_id",
                table: "inventory",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            // 6. Add Indices (only if they don't exist, but here we just add the new ones)
            migrationBuilder.CreateIndex(
                name: "IX_inventory_product_id",
                table: "inventory",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_products_product_id",
                table: "inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventory",
                table: "inventory");

            migrationBuilder.DropIndex(
                name: "IX_inventory_product_id",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "inventory_id",
                table: "inventory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventory",
                table: "inventory",
                columns: new[] { "location_id", "variant_id" });
        }
    }
}
