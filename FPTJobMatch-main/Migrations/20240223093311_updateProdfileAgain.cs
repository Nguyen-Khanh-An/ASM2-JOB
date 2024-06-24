using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTJOB.Migrations
{
    /// <inheritdoc />
    public partial class updateProdfileAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
        // Hoang
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Profile");
        }
    }
}
