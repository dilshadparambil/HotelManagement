using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmployeeHotelNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Hotels_HotelClassId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "HotelClassId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Hotels_HotelClassId",
                table: "Employees",
                column: "HotelClassId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Hotels_HotelClassId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "HotelClassId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Hotels_HotelClassId",
                table: "Employees",
                column: "HotelClassId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
