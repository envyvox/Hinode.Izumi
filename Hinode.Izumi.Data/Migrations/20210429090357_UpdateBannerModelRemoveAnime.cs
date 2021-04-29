using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateBannerModelRemoveAnime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "anime",
                table: "banners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "anime",
                table: "banners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
