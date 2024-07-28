using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeatherMonitoring.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wmapp");

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "wmapp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    region = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true),
                    country = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true),
                    lat = table.Column<double>(type: "double precision", nullable: false),
                    lon = table.Column<double>(type: "double precision", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    included = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                schema: "wmapp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    c_code = table.Column<int>(type: "integer", nullable: false),
                    temp_c = table.Column<double>(type: "double precision", nullable: false),
                    humidity = table.Column<int>(type: "integer", nullable: false),
                    pressure_in = table.Column<double>(type: "double precision", nullable: false),
                    wind_kph = table.Column<double>(type: "double precision", nullable: false),
                    lastup_epoch = table.Column<long>(type: "bigint", nullable: false),
                    lastup = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weather_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "wmapp",
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_Uid",
                schema: "wmapp",
                table: "Location",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weather_LocationId",
                schema: "wmapp",
                table: "Weather",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_Uid",
                schema: "wmapp",
                table: "Weather",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather",
                schema: "wmapp");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "wmapp");
        }
    }
}
