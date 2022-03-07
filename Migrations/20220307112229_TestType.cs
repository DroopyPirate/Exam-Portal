using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Exam_Portal.Migrations
{
    public partial class TestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_TestQuestion_TestQuestion_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Questions_Question_id",
                table: "TestQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Tests_Test_id",
                table: "TestQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion");

            migrationBuilder.RenameTable(
                name: "TestQuestion",
                newName: "TestQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestion_Test_id",
                table: "TestQuestions",
                newName: "IX_TestQuestions_Test_id");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestion_Question_id",
                table: "TestQuestions",
                newName: "IX_TestQuestions_Question_id");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Tests",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Type_id",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Type_id",
                table: "Tests",
                column: "Type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_TestQuestions_TestQuestion_id",
                table: "QuestionResults",
                column: "TestQuestion_id",
                principalTable: "TestQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Questions_Question_id",
                table: "TestQuestions",
                column: "Question_id",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Tests_Test_id",
                table: "TestQuestions",
                column: "Test_id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestTypes_Type_id",
                table: "Tests",
                column: "Type_id",
                principalTable: "TestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_TestQuestions_TestQuestion_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Questions_Question_id",
                table: "TestQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Tests_Test_id",
                table: "TestQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestTypes_Type_id",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "TestTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tests_Type_id",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions");

            migrationBuilder.DropColumn(
                name: "Type_id",
                table: "Tests");

            migrationBuilder.RenameTable(
                name: "TestQuestions",
                newName: "TestQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestions_Test_id",
                table: "TestQuestion",
                newName: "IX_TestQuestion_Test_id");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestions_Question_id",
                table: "TestQuestion",
                newName: "IX_TestQuestion_Question_id");

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Tests",
                type: "text",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_TestQuestion_TestQuestion_id",
                table: "QuestionResults",
                column: "TestQuestion_id",
                principalTable: "TestQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Questions_Question_id",
                table: "TestQuestion",
                column: "Question_id",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Tests_Test_id",
                table: "TestQuestion",
                column: "Test_id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
