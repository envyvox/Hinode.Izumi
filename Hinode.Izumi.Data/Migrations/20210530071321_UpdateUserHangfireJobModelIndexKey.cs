using Microsoft.EntityFrameworkCore.Migrations;

namespace Hinode.Izumi.Data.Migrations
{
    public partial class UpdateUserHangfireJobModelIndexKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_hangfire_jobs_user_id_job_id",
                table: "user_hangfire_jobs");

            migrationBuilder.CreateIndex(
                name: "ix_user_hangfire_jobs_user_id",
                table: "user_hangfire_jobs",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_hangfire_jobs_user_id",
                table: "user_hangfire_jobs");

            migrationBuilder.CreateIndex(
                name: "ix_user_hangfire_jobs_user_id_job_id",
                table: "user_hangfire_jobs",
                columns: new[] { "user_id", "job_id" },
                unique: true);
        }
    }
}
