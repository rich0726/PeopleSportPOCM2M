using Microsoft.EntityFrameworkCore.Migrations;

namespace PeopleSportsSandbox.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Sport = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Sport);
                });

            migrationBuilder.CreateTable(
                name: "PersonSports",
                columns: table => new
                {
                    PeoplePersonID = table.Column<int>(type: "int", nullable: false),
                    SportsSport = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSports", x => new { x.PeoplePersonID, x.SportsSport });
                    table.ForeignKey(
                        name: "FK_PersonSports_People_PeoplePersonID",
                        column: x => x.PeoplePersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSports_Sports_SportsSport",
                        column: x => x.SportsSport,
                        principalTable: "Sports",
                        principalColumn: "Sport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sports",
                column: "Sport",
                values: new object[]
                {
                    "Football",
                    "Soccer",
                    "Baseball",
                    "Basketball",
                    "Tennis",
                    "Golf",
                    "Hockey"
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonSports_SportsSport",
                table: "PersonSports",
                column: "SportsSport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonSports");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
