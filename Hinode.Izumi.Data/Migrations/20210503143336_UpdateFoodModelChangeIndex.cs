using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateFoodModelChangeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_foods_name",
                table: "foods");

            migrationBuilder.CreateIndex(
                name: "ix_foods_name_event",
                table: "foods",
                columns: new[] { "name", "event" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_foods_name_event",
                table: "foods");

            migrationBuilder.CreateIndex(
                name: "ix_foods_name",
                table: "foods",
                column: "name",
                unique: true);
        }
    }
}
