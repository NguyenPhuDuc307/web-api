using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageInLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Lessons");
        }
    }
}
