using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class CreateCurrentWeeklyQuestModelAndUserWeeklyQuestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "weekly_quests");

            migrationBuilder.CreateTable(
                name: "current_weekly_quests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    location = table.Column<int>(type: "integer", nullable: false),
                    quest_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_current_weekly_quests", x => x.id);
                    table.ForeignKey(
                        name: "fk_current_weekly_quests_weekly_quest_quest_id",
                        column: x => x.quest_id,
                        principalTable: "weekly_quests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_weekly_quests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    quest_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_weekly_quests", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_weekly_quests_current_weekly_quests_quest_id",
                        column: x => x.quest_id,
                        principalTable: "current_weekly_quests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_weekly_quests_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_current_weekly_quests_location_quest_id",
                table: "current_weekly_quests",
                columns: new[] { "location", "quest_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_current_weekly_quests_quest_id",
                table: "current_weekly_quests",
                column: "quest_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_weekly_quests_quest_id",
                table: "user_weekly_quests",
                column: "quest_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_weekly_quests_user_id_quest_id",
                table: "user_weekly_quests",
                columns: new[] { "user_id", "quest_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_weekly_quests");

            migrationBuilder.DropTable(
                name: "current_weekly_quests");

            migrationBuilder.AddColumn<int>(
                name: "location",
                table: "weekly_quests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
