using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Access.Layer.Migrations
{
    public partial class timeStampAddedToStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Stocks",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Stocks");
        }
    }
}
