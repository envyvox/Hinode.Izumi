using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    reward = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_achievements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alcohols",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohols", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "banners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    anime = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    effect = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    anime = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "certificates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_certificates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    location = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    currency = table.Column<long>(type: "bigint", nullable: false),
                    reputation = table.Column<long>(type: "bigint", nullable: false),
                    energy = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contracts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "craftings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    location = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_craftings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discord_channels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    channel = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discord_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discord_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discord_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drinks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drinks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "emotes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emotes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "families",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_families", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fishes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    seasons = table.Column<int[]>(type: "integer[]", nullable: false),
                    weather = table.Column<int>(type: "integer", nullable: false),
                    times_day = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fishes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    mastery = table.Column<long>(type: "bigint", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    energy = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_foods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gatherings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    location = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gatherings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "localizations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    single = table.Column<string>(type: "text", nullable: false),
                    @double = table.Column<string>(name: "double", type: "text", nullable: false),
                    multiply = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mastery_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_category = table.Column<int>(type: "integer", nullable: false),
                    property = table.Column<int>(type: "integer", nullable: false),
                    mastery0 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery50 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery100 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery150 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery200 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery250 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mastery_properties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mastery_xp_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property = table.Column<int>(type: "integer", nullable: false),
                    mastery0 = table.Column<double>(type: "double precision", nullable: false),
                    mastery50 = table.Column<double>(type: "double precision", nullable: false),
                    mastery100 = table.Column<double>(type: "double precision", nullable: false),
                    mastery150 = table.Column<double>(type: "double precision", nullable: false),
                    mastery200 = table.Column<double>(type: "double precision", nullable: false),
                    mastery250 = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mastery_xp_properties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seeds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    season = table.Column<int>(type: "integer", nullable: false),
                    growth = table.Column<long>(type: "bigint", nullable: false),
                    re_growth = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    multiply = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seeds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transits",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departure = table.Column<int>(type: "integer", nullable: false),
                    destination = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    about = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    gender = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    location = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    energy = table.Column<int>(type: "integer", nullable: false, defaultValue: 100),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "world_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_category = table.Column<int>(type: "integer", nullable: false),
                    property = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_world_properties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alcohol_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alcohol_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohol_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_alcohol_ingredients_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alcohol_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alcohol_id = table.Column<long>(type: "bigint", nullable: false),
                    property = table.Column<int>(type: "integer", nullable: false),
                    mastery0 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery50 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery100 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery150 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery200 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery250 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohol_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_alcohol_properties_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dynamic_shop_banners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    banner_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dynamic_shop_banners", x => x.id);
                    table.ForeignKey(
                        name: "fk_dynamic_shop_banners_banners_banner_id",
                        column: x => x.banner_id,
                        principalTable: "banners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crafting_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crafting_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crafting_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_crafting_ingredients_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crafting_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crafting_id = table.Column<long>(type: "bigint", nullable: false),
                    property = table.Column<int>(type: "integer", nullable: false),
                    mastery0 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery50 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery100 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery150 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery200 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery250 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crafting_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_crafting_properties_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drink_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    drink_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drink_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_drink_ingredients_drinks_drink_id",
                        column: x => x.drink_id,
                        principalTable: "drinks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "family_currencies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    family_id = table.Column<long>(type: "bigint", nullable: false),
                    currency = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_family_currencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_family_currencies_families_family_id",
                        column: x => x.family_id,
                        principalTable: "families",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_food_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_food_ingredients_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gathering_properties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gathering_id = table.Column<long>(type: "bigint", nullable: false),
                    property = table.Column<int>(type: "integer", nullable: false),
                    mastery0 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery50 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery100 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery150 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery200 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    mastery250 = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gathering_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_gathering_properties_gatherings_gathering_id",
                        column: x => x.gathering_id,
                        principalTable: "gatherings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crops",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_crops_seed_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "family_invites",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    family_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_family_invites", x => x.id);
                    table.ForeignKey(
                        name: "fk_family_invites_families_family_id",
                        column: x => x.family_id,
                        principalTable: "families",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_family_invites_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "market_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    item_id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    selling = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_market_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_market_requests_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    departure = table.Column<int>(type: "integer", nullable: false),
                    destination = table.Column<int>(type: "integer", nullable: false),
                    arrival = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movements", x => x.id);
                    table.ForeignKey(
                        name: "fk_movements_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    achievement_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_achievements", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_achievements_achievements_achievement_id",
                        column: x => x.achievement_id,
                        principalTable: "achievements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_achievements_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_alcohols",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    alcohol_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_alcohols", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_alcohols_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_alcohols_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_banners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    banner_id = table.Column<long>(type: "bigint", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_banners", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_banners_banners_banner_id",
                        column: x => x.banner_id,
                        principalTable: "banners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_banners_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_boxes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    box = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_boxes", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_boxes_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_cards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    card_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cards", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_cards_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_cards_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_certificates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    certificate_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_certificates", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_certificates_certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_certificates_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_collections",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    item_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_collections", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_collections_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_contracts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    contract_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_contracts", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_contracts_contracts_contract_id",
                        column: x => x.contract_id,
                        principalTable: "contracts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_contracts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_cooldowns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    cooldown = table.Column<int>(type: "integer", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cooldowns", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_cooldowns_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_craftings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    crafting_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_craftings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_craftings_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_craftings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_currencies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    currency = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_currencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_currencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_decks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    card_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_decks", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_decks_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_decks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_drinks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    drink_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_drinks", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_drinks_drinks_drink_id",
                        column: x => x.drink_id,
                        principalTable: "drinks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_drinks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_effects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    effect = table.Column<int>(type: "integer", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_effects", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_effects_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_families",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    family_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_families", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_families_family_family_id",
                        column: x => x.family_id,
                        principalTable: "families",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_families_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_fields",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    field_id = table.Column<long>(type: "bigint", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    seed_id = table.Column<long>(type: "bigint", nullable: true),
                    progress = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    re_growth = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_fields", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_fields_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_fields_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_fishes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    fish_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_fishes", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_fishes_fishes_fish_id",
                        column: x => x.fish_id,
                        principalTable: "fishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_fishes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_foods",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_foods", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_foods_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_foods_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_gatherings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    gathering_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_gatherings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_gatherings_gatherings_gathering_id",
                        column: x => x.gathering_id,
                        principalTable: "gatherings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_gatherings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_masteries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    mastery = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_masteries", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_masteries_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_products_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_recipes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    food_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_recipes", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_recipes_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_recipes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_reputations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    reputation = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_reputations", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_reputations_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_seeds",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_seeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_seeds_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_seeds_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_statistics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    statistic = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_statistics", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_statistics_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_titles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_titles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_titles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_trainings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    step = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_trainings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_trainings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_crops",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    crop_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_crops_crops_crop_id",
                        column: x => x.crop_id,
                        principalTable: "crops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_crops_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "family_buildings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    family_id = table.Column<long>(type: "bigint", nullable: false),
                    building_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_family_buildings", x => x.id);
                    table.ForeignKey(
                        name: "fk_family_buildings_family_family_id",
                        column: x => x.family_id,
                        principalTable: "families",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    req_building_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "buildings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: true),
                    category = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buildings", x => x.id);
                    table.ForeignKey(
                        name: "fk_buildings_project_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_ingredients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_ingredients_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_projects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_projects_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_projects_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_buildings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    building_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_buildings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_buildings_buildings_building_id",
                        column: x => x.building_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_buildings_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_achievements_type",
                table: "achievements",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_alcohol_ingredients_alcohol_id_category_ingredient_id",
                table: "alcohol_ingredients",
                columns: new[] { "alcohol_id", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_alcohol_properties_alcohol_id_property",
                table: "alcohol_properties",
                columns: new[] { "alcohol_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_alcohols_name",
                table: "alcohols",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_banners_name",
                table: "banners",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_buildings_project_id",
                table: "buildings",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_buildings_type",
                table: "buildings",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cards_name",
                table: "cards",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_certificates_type",
                table: "certificates",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contracts_name",
                table: "contracts",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crafting_ingredients_crafting_id_category_ingredient_id",
                table: "crafting_ingredients",
                columns: new[] { "crafting_id", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crafting_properties_crafting_id_property",
                table: "crafting_properties",
                columns: new[] { "crafting_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_craftings_name",
                table: "craftings",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crops_name",
                table: "crops",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crops_seed_id",
                table: "crops",
                column: "seed_id");

            migrationBuilder.CreateIndex(
                name: "ix_discord_channels_channel",
                table: "discord_channels",
                column: "channel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_discord_roles_role",
                table: "discord_roles",
                column: "role",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drink_ingredients_drink_id_category_ingredient_id",
                table: "drink_ingredients",
                columns: new[] { "drink_id", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drinks_name",
                table: "drinks",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dynamic_shop_banners_banner_id",
                table: "dynamic_shop_banners",
                column: "banner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_emotes_id_name",
                table: "emotes",
                columns: new[] { "id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_families_name",
                table: "families",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_family_buildings_building_id",
                table: "family_buildings",
                column: "building_id");

            migrationBuilder.CreateIndex(
                name: "ix_family_buildings_family_id_building_id",
                table: "family_buildings",
                columns: new[] { "family_id", "building_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_family_currencies_family_id_currency",
                table: "family_currencies",
                columns: new[] { "family_id", "currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_family_invites_family_id_user_id",
                table: "family_invites",
                columns: new[] { "family_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_family_invites_user_id",
                table: "family_invites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_fishes_name",
                table: "fishes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_food_ingredients_food_id_category_ingredient_id",
                table: "food_ingredients",
                columns: new[] { "food_id", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_foods_name",
                table: "foods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gathering_properties_gathering_id_property",
                table: "gathering_properties",
                columns: new[] { "gathering_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gatherings_name",
                table: "gatherings",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_type",
                table: "images",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_localizations_name",
                table: "localizations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_market_requests_user_id_category_item_id",
                table: "market_requests",
                columns: new[] { "user_id", "category", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mastery_properties_property",
                table: "mastery_properties",
                column: "property",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mastery_xp_properties_property",
                table: "mastery_xp_properties",
                column: "property",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movements_user_id",
                table: "movements",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_ingredients_project_id_category_ingredient_id",
                table: "project_ingredients",
                columns: new[] { "project_id", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_projects_name",
                table: "projects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_projects_req_building_id",
                table: "projects",
                column: "req_building_id");

            migrationBuilder.CreateIndex(
                name: "ix_seeds_name",
                table: "seeds",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transits_departure_destination",
                table: "transits",
                columns: new[] { "departure", "destination" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_achievements_user_id_achievement_id",
                table: "user_achievements",
                columns: new[] { "user_id", "achievement_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_alcohols_alcohol_id",
                table: "user_alcohols",
                column: "alcohol_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_alcohols_user_id_alcohol_id",
                table: "user_alcohols",
                columns: new[] { "user_id", "alcohol_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_banners_banner_id",
                table: "user_banners",
                column: "banner_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_banners_user_id_banner_id",
                table: "user_banners",
                columns: new[] { "user_id", "banner_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_boxes_user_id_box",
                table: "user_boxes",
                columns: new[] { "user_id", "box" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_buildings_building_id",
                table: "user_buildings",
                column: "building_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_buildings_user_id_building_id",
                table: "user_buildings",
                columns: new[] { "user_id", "building_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_cards_card_id",
                table: "user_cards",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_cards_user_id_card_id",
                table: "user_cards",
                columns: new[] { "user_id", "card_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_certificates_certificate_id",
                table: "user_certificates",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_certificates_user_id_certificate_id",
                table: "user_certificates",
                columns: new[] { "user_id", "certificate_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_collections_user_id_category_item_id",
                table: "user_collections",
                columns: new[] { "user_id", "category", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_contracts_contract_id",
                table: "user_contracts",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_contracts_user_id",
                table: "user_contracts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_cooldowns_user_id_cooldown",
                table: "user_cooldowns",
                columns: new[] { "user_id", "cooldown" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_craftings_crafting_id",
                table: "user_craftings",
                column: "crafting_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_craftings_user_id_crafting_id",
                table: "user_craftings",
                columns: new[] { "user_id", "crafting_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_crop_id",
                table: "user_crops",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_user_id_crop_id",
                table: "user_crops",
                columns: new[] { "user_id", "crop_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_currencies_user_id_currency",
                table: "user_currencies",
                columns: new[] { "user_id", "currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_decks_card_id",
                table: "user_decks",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_decks_user_id_card_id",
                table: "user_decks",
                columns: new[] { "user_id", "card_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_drinks_drink_id",
                table: "user_drinks",
                column: "drink_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_drinks_user_id_drink_id",
                table: "user_drinks",
                columns: new[] { "user_id", "drink_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_effects_user_id_effect",
                table: "user_effects",
                columns: new[] { "user_id", "effect" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_families_family_id",
                table: "user_families",
                column: "family_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_families_user_id",
                table: "user_families",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_fields_seed_id",
                table: "user_fields",
                column: "seed_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_fields_user_id_field_id",
                table: "user_fields",
                columns: new[] { "user_id", "field_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_fish_id",
                table: "user_fishes",
                column: "fish_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_user_id_fish_id",
                table: "user_fishes",
                columns: new[] { "user_id", "fish_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_foods_food_id",
                table: "user_foods",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_foods_user_id_food_id",
                table: "user_foods",
                columns: new[] { "user_id", "food_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_gatherings_gathering_id",
                table: "user_gatherings",
                column: "gathering_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_gatherings_user_id_gathering_id",
                table: "user_gatherings",
                columns: new[] { "user_id", "gathering_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_masteries_user_id_mastery",
                table: "user_masteries",
                columns: new[] { "user_id", "mastery" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_products_product_id",
                table: "user_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_products_user_id_product_id",
                table: "user_products",
                columns: new[] { "user_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_projects_project_id",
                table: "user_projects",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_projects_user_id_project_id",
                table: "user_projects",
                columns: new[] { "user_id", "project_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_recipes_food_id",
                table: "user_recipes",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_recipes_user_id_food_id",
                table: "user_recipes",
                columns: new[] { "user_id", "food_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_reputations_user_id_reputation",
                table: "user_reputations",
                columns: new[] { "user_id", "reputation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_seed_id",
                table: "user_seeds",
                column: "seed_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_user_id_seed_id",
                table: "user_seeds",
                columns: new[] { "user_id", "seed_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_statistics_user_id_statistic",
                table: "user_statistics",
                columns: new[] { "user_id", "statistic" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_titles_user_id_title",
                table: "user_titles",
                columns: new[] { "user_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_trainings_user_id",
                table: "user_trainings",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_world_properties_property",
                table: "world_properties",
                column: "property",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_family_buildings_buildings_building_id",
                table: "family_buildings",
                column: "building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_buildings_project_project_id1",
                table: "projects",
                column: "req_building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_buildings_project_project_id",
                table: "buildings");

            migrationBuilder.DropTable(
                name: "alcohol_ingredients");

            migrationBuilder.DropTable(
                name: "alcohol_properties");

            migrationBuilder.DropTable(
                name: "crafting_ingredients");

            migrationBuilder.DropTable(
                name: "crafting_properties");

            migrationBuilder.DropTable(
                name: "discord_channels");

            migrationBuilder.DropTable(
                name: "discord_roles");

            migrationBuilder.DropTable(
                name: "drink_ingredients");

            migrationBuilder.DropTable(
                name: "dynamic_shop_banners");

            migrationBuilder.DropTable(
                name: "emotes");

            migrationBuilder.DropTable(
                name: "family_buildings");

            migrationBuilder.DropTable(
                name: "family_currencies");

            migrationBuilder.DropTable(
                name: "family_invites");

            migrationBuilder.DropTable(
                name: "food_ingredients");

            migrationBuilder.DropTable(
                name: "gathering_properties");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "localizations");

            migrationBuilder.DropTable(
                name: "market_requests");

            migrationBuilder.DropTable(
                name: "mastery_properties");

            migrationBuilder.DropTable(
                name: "mastery_xp_properties");

            migrationBuilder.DropTable(
                name: "movements");

            migrationBuilder.DropTable(
                name: "project_ingredients");

            migrationBuilder.DropTable(
                name: "transits");

            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "user_alcohols");

            migrationBuilder.DropTable(
                name: "user_banners");

            migrationBuilder.DropTable(
                name: "user_boxes");

            migrationBuilder.DropTable(
                name: "user_buildings");

            migrationBuilder.DropTable(
                name: "user_cards");

            migrationBuilder.DropTable(
                name: "user_certificates");

            migrationBuilder.DropTable(
                name: "user_collections");

            migrationBuilder.DropTable(
                name: "user_contracts");

            migrationBuilder.DropTable(
                name: "user_cooldowns");

            migrationBuilder.DropTable(
                name: "user_craftings");

            migrationBuilder.DropTable(
                name: "user_crops");

            migrationBuilder.DropTable(
                name: "user_currencies");

            migrationBuilder.DropTable(
                name: "user_decks");

            migrationBuilder.DropTable(
                name: "user_drinks");

            migrationBuilder.DropTable(
                name: "user_effects");

            migrationBuilder.DropTable(
                name: "user_families");

            migrationBuilder.DropTable(
                name: "user_fields");

            migrationBuilder.DropTable(
                name: "user_fishes");

            migrationBuilder.DropTable(
                name: "user_foods");

            migrationBuilder.DropTable(
                name: "user_gatherings");

            migrationBuilder.DropTable(
                name: "user_masteries");

            migrationBuilder.DropTable(
                name: "user_products");

            migrationBuilder.DropTable(
                name: "user_projects");

            migrationBuilder.DropTable(
                name: "user_recipes");

            migrationBuilder.DropTable(
                name: "user_reputations");

            migrationBuilder.DropTable(
                name: "user_seeds");

            migrationBuilder.DropTable(
                name: "user_statistics");

            migrationBuilder.DropTable(
                name: "user_titles");

            migrationBuilder.DropTable(
                name: "user_trainings");

            migrationBuilder.DropTable(
                name: "world_properties");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "alcohols");

            migrationBuilder.DropTable(
                name: "banners");

            migrationBuilder.DropTable(
                name: "certificates");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "craftings");

            migrationBuilder.DropTable(
                name: "crops");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "drinks");

            migrationBuilder.DropTable(
                name: "families");

            migrationBuilder.DropTable(
                name: "fishes");

            migrationBuilder.DropTable(
                name: "gatherings");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "foods");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "seeds");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "buildings");
        }
    }
}
