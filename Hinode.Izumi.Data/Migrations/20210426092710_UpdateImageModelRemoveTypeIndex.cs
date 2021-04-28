using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateImageModelRemoveTypeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_images_type",
                table: "images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_images_type",
                table: "images",
                column: "type",
                unique: true);
        }
    }
}
