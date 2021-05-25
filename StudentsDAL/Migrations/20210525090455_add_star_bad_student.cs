using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentsDAL.Migrations
{
    public partial class add_star_bad_student : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarStudents_Students_Id",
                        column: x => x.Id,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarStudents");
        }
    }
}
