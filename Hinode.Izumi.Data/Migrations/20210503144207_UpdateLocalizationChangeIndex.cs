using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateLocalizationChangeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_localizations_name",
                table: "localizations");

            migrationBuilder.CreateIndex(
                name: "ix_localizations_category_item_id",
                table: "localizations",
                columns: new[] { "category", "item_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_localizations_category_item_id",
                table: "localizations");

            migrationBuilder.CreateIndex(
                name: "ix_localizations_name",
                table: "localizations",
                column: "name",
                unique: true);
        }
    }
}
