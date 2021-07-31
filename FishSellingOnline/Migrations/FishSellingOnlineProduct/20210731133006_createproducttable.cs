using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FishSellingOnline.Migrations.FishSellingOnlineProduct
{
    public partial class createproducttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: true),
                    ProductImage = table.Column<byte[]>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    ProductPrice = table.Column<decimal>(nullable: false),
                    stockleft = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
