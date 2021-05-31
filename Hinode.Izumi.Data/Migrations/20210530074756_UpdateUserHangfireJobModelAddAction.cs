using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateUserHangfireJobModelAddAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_hangfire_jobs_user_id",
                table: "user_hangfire_jobs");

            migrationBuilder.AddColumn<int>(
                name: "action",
                table: "user_hangfire_jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_user_hangfire_jobs_user_id_action_job_id",
                table: "user_hangfire_jobs",
                columns: new[] { "user_id", "action", "job_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_hangfire_jobs_user_id_action_job_id",
                table: "user_hangfire_jobs");

            migrationBuilder.DropColumn(
                name: "action",
                table: "user_hangfire_jobs");

            migrationBuilder.CreateIndex(
                name: "ix_user_hangfire_jobs_user_id",
                table: "user_hangfire_jobs",
                column: "user_id",
                unique: true);
        }
    }
}
