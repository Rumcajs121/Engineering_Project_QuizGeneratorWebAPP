using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexattemptquiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QuizAttemptQuestions_Attempt_Question_UQ",
                table: "QuizAttemptQuestions",
                columns: new[] { "QuizAttemptId", "QuizQuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuizAttemptQuestions_Attempt_Question_UQ",
                table: "QuizAttemptQuestions");
        }
    }
}
