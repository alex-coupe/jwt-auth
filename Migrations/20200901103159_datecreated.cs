using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace JwtAuth.Migrations
{
    public partial class datecreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
              name: "DateCreated",
              table: "Users",
              nullable: true
              );
                    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.DropColumn("DateCreated", "Users");
        }
    }
}
