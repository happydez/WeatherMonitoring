using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherMonitoring.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class WeatherUidIgnore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Weather_Uid",
                schema: "wmapp",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "wmapp",
                table: "Weather");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uid",
                schema: "wmapp",
                table: "Weather",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Weather_Uid",
                schema: "wmapp",
                table: "Weather",
                column: "Uid",
                unique: true);
        }
    }
}
