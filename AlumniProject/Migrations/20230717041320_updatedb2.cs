using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniProject.Migrations
{
    public partial class updatedb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    schoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Attributes_School_schoolId",
                        column: x => x.schoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_schoolId",
                table: "Attributes",
                column: "schoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attributes");
        }
    }
}
