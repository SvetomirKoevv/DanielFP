using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Cars_CarId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_CarId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Image");

            migrationBuilder.CreateIndex(
                name: "IX_Image_VehicleId",
                table: "Image",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Cars_VehicleId",
                table: "Image",
                column: "VehicleId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Cars_VehicleId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_VehicleId",
                table: "Image");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Image_CarId",
                table: "Image",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Cars_CarId",
                table: "Image",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
