using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateLocalizationModelAddItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "item_id",
                table: "localizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item_id",
                table: "localizations");
        }
    }
}
