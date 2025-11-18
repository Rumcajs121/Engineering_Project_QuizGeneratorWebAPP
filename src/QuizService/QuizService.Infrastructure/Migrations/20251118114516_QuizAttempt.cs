using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuizAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizAttemptQuestions",
                table: "QuizAttemptQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizAttemptQuestions",
                table: "QuizAttemptQuestions",
                column: "QuizAttemptQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizAttemptQuestions",
                table: "QuizAttemptQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizAttemptQuestions",
                table: "QuizAttemptQuestions",
                columns: new[] { "QuizAttemptId", "QuizAttemptQuestionId" });
        }
    }
}
