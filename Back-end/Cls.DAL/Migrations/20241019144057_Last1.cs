using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cls.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Last1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ApplyDoctorRequests");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ApplyDoctorRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ApplyDoctorRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ApplyDoctorRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
