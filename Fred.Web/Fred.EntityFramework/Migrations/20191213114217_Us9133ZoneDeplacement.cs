﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Fred.EntityFramework.Migrations
{
    public partial class Us9133ZoneDeplacement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCadre",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT");

            migrationBuilder.DropColumn(
                name: "IsETAM",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT");

            migrationBuilder.DropColumn(
                name: "IsOuvrier",
                schema: "dbo",
                table: "FRED_CODE_ZONE_DEPLACEMENT");
        }
    }
}
