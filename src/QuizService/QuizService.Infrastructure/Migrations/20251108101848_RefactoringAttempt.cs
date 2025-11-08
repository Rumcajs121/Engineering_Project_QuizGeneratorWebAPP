using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Quizzes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "QuizAttempts",
                newName: "SubmittedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Quizzes",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "QuizAttempts",
                newName: "EndTime");
        }
    }
}
