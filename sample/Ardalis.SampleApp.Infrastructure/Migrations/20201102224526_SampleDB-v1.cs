using Microsoft.EntityFrameworkCore.Migrations;

namespace Ardalis.SampleApp.Infrastructure.Migrations;

public partial class SampleDBv1 : Migration
{
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "Customer",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
          Name = table.Column<string>(nullable: true),
          Email = table.Column<string>(nullable: true),
          Address = table.Column<string>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Customer", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "Store",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
          Name = table.Column<string>(nullable: true),
          Address = table.Column<string>(nullable: true),
          CustomerId = table.Column<int>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Store", x => x.Id);
          table.ForeignKey(
                      name: "FK_Store_Customer_CustomerId",
                      column: x => x.CustomerId,
                      principalTable: "Customer",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_Store_CustomerId",
        table: "Store",
        column: "CustomerId");
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "Store");

    migrationBuilder.DropTable(
        name: "Customer");
  }
}
