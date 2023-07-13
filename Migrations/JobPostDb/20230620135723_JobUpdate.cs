using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Post_Website.Migrations.JobPostDb
{
    /// <inheritdoc />
    public partial class JobUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobPosterId",
                table: "Job",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobPosterNormalizedUserName",
                table: "Job",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobPosterId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "JobPosterNormalizedUserName",
                table: "Job");
        }
    }
}
