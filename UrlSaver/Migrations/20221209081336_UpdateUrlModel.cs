using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlSaver.Migrations
{
    public partial class UpdateUrlModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Urls",
                newName: "Original");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Urls",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Hostname",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TopDomain",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubDomains",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hostname",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "Original",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "SubDomains",
                table: "Urls");

            migrationBuilder.RenameColumn(
                name: "TopDomain",
                table: "Urls",
                newName: "Link");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Urls",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
