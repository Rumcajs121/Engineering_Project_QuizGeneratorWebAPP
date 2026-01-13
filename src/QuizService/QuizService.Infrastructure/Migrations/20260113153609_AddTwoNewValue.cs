using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoNewValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
