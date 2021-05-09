using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateContentVoteIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_content_votes_user_id_message_id",
                table: "content_votes");

            migrationBuilder.CreateIndex(
                name: "ix_content_votes_user_id_message_id_vote",
                table: "content_votes",
                columns: new[] { "user_id", "message_id", "vote" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_content_votes_user_id_message_id_vote",
                table: "content_votes");

            migrationBuilder.CreateIndex(
                name: "ix_content_votes_user_id_message_id",
                table: "content_votes",
                columns: new[] { "user_id", "message_id" },
                unique: true);
        }
    }
}
