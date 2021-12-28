using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam_Portal.Migrations
{
    public partial class ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Result",
                table: "TotalResults",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalResults_Test_id",
                table: "TotalResults",
                column: "Test_id");

            migrationBuilder.CreateIndex(
                name: "IX_TotalResults_User_id",
                table: "TotalResults",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Faculty_id",
                table: "Tests",
                column: "Faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Tag_id",
                table: "Questions",
                column: "Tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Test_id",
                table: "Questions",
                column: "Test_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_Option_id",
                table: "QuestionResults",
                column: "Option_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_Question_id",
                table: "QuestionResults",
                column: "Question_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_User_id",
                table: "QuestionResults",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Options_Question_id",
                table: "Options",
                column: "Question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_User_id",
                table: "Groups",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTests_Group_id",
                table: "AssignedTests",
                column: "Group_id");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTests_Test_id",
                table: "AssignedTests",
                column: "Test_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTests_Groups_Group_id",
                table: "AssignedTests",
                column: "Group_id",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTests_Tests_Test_id",
                table: "AssignedTests",
                column: "Test_id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_User_id",
                table: "Groups",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_Question_id",
                table: "Options",
                column: "Question_id",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_Options_Option_id",
                table: "QuestionResults",
                column: "Option_id",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_Questions_Question_id",
                table: "QuestionResults",
                column: "Question_id",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_Users_User_id",
                table: "QuestionResults",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tags_Tag_id",
                table: "Questions",
                column: "Tag_id",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tests_Test_id",
                table: "Questions",
                column: "Test_id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Users_Faculty_id",
                table: "Tests",
                column: "Faculty_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TotalResults_Tests_Test_id",
                table: "TotalResults",
                column: "Test_id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TotalResults_Users_User_id",
                table: "TotalResults",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTests_Groups_Group_id",
                table: "AssignedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTests_Tests_Test_id",
                table: "AssignedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_User_id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_Question_id",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_Options_Option_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_Questions_Question_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_Users_User_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tags_Tag_id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tests_Test_id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Users_Faculty_id",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TotalResults_Tests_Test_id",
                table: "TotalResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TotalResults_Users_User_id",
                table: "TotalResults");

            migrationBuilder.DropIndex(
                name: "IX_TotalResults_Test_id",
                table: "TotalResults");

            migrationBuilder.DropIndex(
                name: "IX_TotalResults_User_id",
                table: "TotalResults");

            migrationBuilder.DropIndex(
                name: "IX_Tests_Faculty_id",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Tag_id",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Test_id",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResults_Option_id",
                table: "QuestionResults");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResults_Question_id",
                table: "QuestionResults");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResults_User_id",
                table: "QuestionResults");

            migrationBuilder.DropIndex(
                name: "IX_Options_Question_id",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Groups_User_id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_AssignedTests_Group_id",
                table: "AssignedTests");

            migrationBuilder.DropIndex(
                name: "IX_AssignedTests_Test_id",
                table: "AssignedTests");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "TotalResults",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }
    }
}
