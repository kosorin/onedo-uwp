using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneDo.Model.Migrations
{
    public partial class FolderSort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Deleted",
            //    table: "Todos");

            //migrationBuilder.DropColumn(
            //    name: "Updated",
            //    table: "Todos");

            migrationBuilder.AddColumn<Guid>(
                name: "Left",
                table: "Folders",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Right",
                table: "Folders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Left",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "Folders");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Deleted",
            //    table: "Todos",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Updated",
            //    table: "Todos",
            //    nullable: true);
        }
    }
}
