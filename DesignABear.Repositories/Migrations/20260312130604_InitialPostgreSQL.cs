using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesignABear.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bear_devices",
                columns: table => new
                {
                    device_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    serial_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    firmware_version = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    color = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    manufactured_date = table.Column<DateOnly>(type: "date", nullable: true),
                    warranty_expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    mac_address = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: true),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bear_devices", x => x.device_id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    slug = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    slug = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    license_brand = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characters", x => x.character_id);
                });

            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    slug = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collections", x => x.collection_id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    location_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    location_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    address_text = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.location_id);
                });

            migrationBuilder.CreateTable(
                name: "personalization_groups",
                columns: table => new
                {
                    group_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalization_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    product_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_personalizable = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    model_3d_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    promotion_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    discount_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    starts_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ends_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.promotion_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    color_hex = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    icon = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "personalization_rules",
                columns: table => new
                {
                    rule_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    base_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    addon_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rule_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    max_qty = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalization_rules", x => x.rule_id);
                    table.ForeignKey(
                        name: "FK_personalization_rules_personalization_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "personalization_groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_personalization_rules_products_addon_product_id",
                        column: x => x.addon_product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personalization_rules_products_base_product_id",
                        column: x => x.base_product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => new { x.product_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_product_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_categories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_characters",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    character_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_characters", x => new { x.product_id, x.character_id });
                    table.ForeignKey(
                        name: "FK_product_characters_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "character_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_characters_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_collections",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_collections", x => new { x.product_id, x.collection_id });
                    table.ForeignKey(
                        name: "FK_product_collections_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "collections",
                        principalColumn: "collection_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_collections_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_media",
                columns: table => new
                {
                    media_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    alt_text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_media", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_product_media_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    variant_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_sold_out = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.variant_id);
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    provider_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    timezone = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_login_count = table.Column<int>(type: "integer", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    on_hand = table.Column<int>(type: "integer", nullable: false),
                    reserved = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => new { x.location_id, x.variant_id });
                    table.ForeignKey(
                        name: "FK_inventory_locations_location_id",
                        column: x => x.location_id,
                        principalTable: "locations",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_product_variants_variant_id",
                        column: x => x.variant_id,
                        principalTable: "product_variants",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    address_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    label = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    full_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    line1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    line2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    state = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    is_default_shipping = table.Column<bool>(type: "boolean", nullable: false),
                    is_default_billing = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_addresses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "builds",
                columns: table => new
                {
                    build_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    base_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    build_name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    personalization_note = table.Column<string>(type: "text", nullable: true),
                    total_weight_gram = table.Column<float>(type: "real", nullable: true),
                    calculated_price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_builds", x => x.build_id);
                    table.ForeignKey(
                        name: "FK_builds_product_variants_base_variant_id",
                        column: x => x.base_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_builds_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_carts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user_subscriptions",
                columns: table => new
                {
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plan_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 4, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    auto_renew = table.Column<bool>(type: "boolean", nullable: false),
                    last_renewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancellation_reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    promo_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_subscriptions", x => x.subscription_id);
                    table.ForeignKey(
                        name: "FK_user_subscriptions_bear_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "bear_devices",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    shipping_address_id = table.Column<Guid>(type: "uuid", nullable: true),
                    billing_address_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    discount_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    tax_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    shipping_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    grand_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_addresses_billing_address_id",
                        column: x => x.billing_address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_addresses_shipping_address_id",
                        column: x => x.shipping_address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "build_components",
                columns: table => new
                {
                    build_component_id = table.Column<Guid>(type: "uuid", nullable: false),
                    build_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    option_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    additional_price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_build_components", x => x.build_component_id);
                    table.ForeignKey(
                        name: "FK_build_components_builds_build_id",
                        column: x => x.build_id,
                        principalTable: "builds",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_build_components_personalization_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "personalization_groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_build_components_product_variants_option_variant_id",
                        column: x => x.option_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    cart_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    build_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price_snapshot = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => x.cart_item_id);
                    table.ForeignKey(
                        name: "FK_cart_items_builds_build_id",
                        column: x => x.build_id,
                        principalTable: "builds",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cart_items_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_items_product_variants_variant_id",
                        column: x => x.variant_id,
                        principalTable: "product_variants",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fulfillments",
                columns: table => new
                {
                    fulfillment_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    tracking_number = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    fulfilled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fulfillments", x => x.fulfillment_id);
                    table.ForeignKey(
                        name: "FK_fulfillments_locations_location_id",
                        column: x => x.location_id,
                        principalTable: "locations",
                        principalColumn: "location_id");
                    table.ForeignKey(
                        name: "FK_fulfillments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    build_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    line_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    product_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    personalization_snapshot = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.order_item_id);
                    table.ForeignKey(
                        name: "FK_order_items_builds_build_id",
                        column: x => x.build_id,
                        principalTable: "builds",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_product_variants_variant_id",
                        column: x => x.variant_id,
                        principalTable: "product_variants",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_promotions",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_promotions", x => new { x.order_id, x.promotion_id });
                    table.ForeignKey(
                        name: "FK_order_promotions_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_promotions_promotions_promotion_id",
                        column: x => x.promotion_id,
                        principalTable: "promotions",
                        principalColumn: "promotion_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    external_txn_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    payment_url = table.Column<string>(type: "text", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    webhook_raw = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "production_jobs",
                columns: table => new
                {
                    job_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    build_id = table.Column<Guid>(type: "uuid", nullable: true),
                    technician_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    material_consumption_gram = table.Column<float>(type: "real", nullable: true),
                    actual_production_cost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    print_data = table.Column<string>(type: "jsonb", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    batch_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    machine_id = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    estimated_duration_min = table.Column<int>(type: "integer", nullable: true),
                    actual_duration_min = table.Column<int>(type: "integer", nullable: true),
                    quality_score = table.Column<int>(type: "integer", nullable: true),
                    rejection_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_jobs", x => x.job_id);
                    table.ForeignKey(
                        name: "FK_production_jobs_builds_build_id",
                        column: x => x.build_id,
                        principalTable: "builds",
                        principalColumn: "build_id");
                    table.ForeignKey(
                        name: "FK_production_jobs_order_items_order_item_id",
                        column: x => x.order_item_id,
                        principalTable: "order_items",
                        principalColumn: "order_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_production_jobs_users_technician_id",
                        column: x => x.technician_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_production_jobs_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_reviews_order_items_order_item_id",
                        column: x => x.order_item_id,
                        principalTable: "order_items",
                        principalColumn: "order_item_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "review_replies",
                columns: table => new
                {
                    reply_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    review_id = table.Column<Guid>(type: "uuid", nullable: false),
                    staff_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_replies", x => x.reply_id);
                    table.ForeignKey(
                        name: "FK_review_replies_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_replies_users_staff_user_id",
                        column: x => x.staff_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_user_id",
                table: "addresses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_bear_devices_serial_number",
                table: "bear_devices",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_build_components_build_id",
                table: "build_components",
                column: "build_id");

            migrationBuilder.CreateIndex(
                name: "IX_build_components_group_id",
                table: "build_components",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_build_components_option_variant_id",
                table: "build_components",
                column: "option_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_builds_base_variant_id",
                table: "builds",
                column: "base_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_builds_user_id",
                table: "builds",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_build_id",
                table: "cart_items",
                column: "build_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_cart_id",
                table: "cart_items",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_variant_id",
                table: "cart_items",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_carts_user_id",
                table: "carts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_fulfillments_location_id",
                table: "fulfillments",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_fulfillments_order_id",
                table: "fulfillments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_variant_id",
                table: "inventory",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_build_id",
                table: "order_items",
                column: "build_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_variant_id",
                table: "order_items",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_promotions_promotion_id",
                table: "order_promotions",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_billing_address_id",
                table: "orders",
                column: "billing_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_shipping_address_id",
                table: "orders",
                column: "shipping_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status",
                table: "orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_orders_updated_by",
                table: "orders",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_order_id",
                table: "payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_personalization_rules_addon_product_id",
                table: "personalization_rules",
                column: "addon_product_id");

            migrationBuilder.CreateIndex(
                name: "IX_personalization_rules_base_product_id_addon_product_id_grou~",
                table: "personalization_rules",
                columns: new[] { "base_product_id", "addon_product_id", "group_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personalization_rules_group_id",
                table: "personalization_rules",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_category_id",
                table: "product_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_characters_character_id",
                table: "product_characters",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_collections_collection_id",
                table: "product_collections",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_media_product_id",
                table: "product_media",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_production_jobs_build_id",
                table: "production_jobs",
                column: "build_id");

            migrationBuilder.CreateIndex(
                name: "IX_production_jobs_order_item_id",
                table: "production_jobs",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_production_jobs_status",
                table: "production_jobs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_production_jobs_technician_id",
                table: "production_jobs",
                column: "technician_id");

            migrationBuilder.CreateIndex(
                name: "IX_production_jobs_updated_by",
                table: "production_jobs",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_review_replies_review_id",
                table: "review_replies",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_replies_staff_user_id",
                table: "review_replies",
                column: "staff_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_order_item_id",
                table: "reviews",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_product_id",
                table: "reviews",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_device_id",
                table: "user_subscriptions",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_end_date",
                table: "user_subscriptions",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "IX_user_subscriptions_user_status",
                table: "user_subscriptions",
                columns: new[] { "user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_token",
                table: "user_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_user_id",
                table: "user_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "build_components");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "fulfillments");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "order_promotions");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "personalization_rules");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "product_characters");

            migrationBuilder.DropTable(
                name: "product_collections");

            migrationBuilder.DropTable(
                name: "product_media");

            migrationBuilder.DropTable(
                name: "production_jobs");

            migrationBuilder.DropTable(
                name: "review_replies");

            migrationBuilder.DropTable(
                name: "user_subscriptions");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "personalization_groups");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "collections");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "bear_devices");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "builds");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
