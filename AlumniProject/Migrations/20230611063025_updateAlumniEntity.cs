using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniProject.Migrations
{
    public partial class updateAlumniEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumni_School_schoolId",
                table: "Alumni");

            migrationBuilder.DropIndex(
                name: "IX_Alumni_schoolId",
                table: "Alumni");

            migrationBuilder.AlterColumn<int>(
                name: "schoolId",
                table: "Alumni",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_schoolId",
                table: "Alumni",
                column: "schoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumni_School_schoolId",
                table: "Alumni",
                column: "schoolId",
                principalTable: "School",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumni_School_schoolId",
                table: "Alumni");

            migrationBuilder.DropIndex(
                name: "IX_Alumni_schoolId",
                table: "Alumni");

            migrationBuilder.AlterColumn<int>(
                name: "schoolId",
                table: "Alumni",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_schoolId",
                table: "Alumni",
                column: "schoolId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alumni_School_schoolId",
                table: "Alumni",
                column: "schoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
