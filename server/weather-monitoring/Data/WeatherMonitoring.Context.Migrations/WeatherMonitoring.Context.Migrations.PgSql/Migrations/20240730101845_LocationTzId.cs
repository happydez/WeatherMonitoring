using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherMonitoring.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class LocationTzId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tz_id",
                schema: "wmapp",
                table: "Location",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tz_id",
                schema: "wmapp",
                table: "Location");
        }
    }
}
