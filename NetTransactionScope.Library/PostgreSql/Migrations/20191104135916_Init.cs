using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetTransactionScope.Library.PostgreSql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: false),
                    path = table.Column<string>(nullable: false),
                    author = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
