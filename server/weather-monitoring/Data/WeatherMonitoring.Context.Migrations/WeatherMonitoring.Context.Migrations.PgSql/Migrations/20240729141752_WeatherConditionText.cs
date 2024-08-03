using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherMonitoring.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class WeatherConditionText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "c_text",
                schema: "wmapp",
                table: "Weather",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "c_text",
                schema: "wmapp",
                table: "Weather");
        }
    }
}
