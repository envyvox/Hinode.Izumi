using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateBannerModelRemoveNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_banners_name",
                table: "banners");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "banners",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "banners",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_banners_name",
                table: "banners",
                column: "name",
                unique: true);
        }
    }
}
