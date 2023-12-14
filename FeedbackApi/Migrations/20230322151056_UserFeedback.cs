using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackApi.Migrations
{
    /// <inheritdoc />
    public partial class UserFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userfeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ratings = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedback", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFeedback");
        }
    }
}
