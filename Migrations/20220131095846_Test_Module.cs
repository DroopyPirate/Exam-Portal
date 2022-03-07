using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Exam_Portal.Migrations
{
    public partial class Test_Module : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_Questions_Question_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tests_Test_id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroup_AspNetUsers_User_id",
                table: "UserGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroup_Groups_Group_id",
                table: "UserGroup");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Test_id",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResults_Question_id",
                table: "QuestionResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroup",
                table: "UserGroup");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Test_id",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Answer_time",
                table: "QuestionResults");

            migrationBuilder.RenameTable(
                name: "UserGroup",
                newName: "UserGroups");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Tests",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Question_id",
                table: "QuestionResults",
                newName: "TestQuestions_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroup_User_id",
                table: "UserGroups",
                newName: "IX_UserGroups_User_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroup_Group_id",
                table: "UserGroups",
                newName: "IX_UserGroups_Group_id");

            migrationBuilder.AlterColumn<int>(
                name: "PassingMarks",
                table: "Tests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Marks",
                table: "Tests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Tests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Tests",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Creator_id",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestQuestion_id",
                table: "QuestionResults",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Semester",
                table: "Groups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Creator_id",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TestQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Test_id = table.Column<int>(type: "int", nullable: false),
                    Question_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestion_Questions_Question_id",
                        column: x => x.Question_id,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestQuestion_Tests_Test_id",
                        column: x => x.Test_id,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Creator_id",
                table: "Tags",
                column: "Creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_TestQuestion_id",
                table: "QuestionResults",
                column: "TestQuestion_id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Creator_id",
                table: "Groups",
                column: "Creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestion_Question_id",
                table: "TestQuestion",
                column: "Question_id");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestion_Test_id",
                table: "TestQuestion",
                column: "Test_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_Creator_id",
                table: "Groups",
                column: "Creator_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_TestQuestion_TestQuestion_id",
                table: "QuestionResults",
                column: "TestQuestion_id",
                principalTable: "TestQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_Creator_id",
                table: "Tags",
                column: "Creator_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_User_id",
                table: "UserGroups",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_Group_id",
                table: "UserGroups",
                column: "Group_id",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_Creator_id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionResults_TestQuestion_TestQuestion_id",
                table: "QuestionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_Creator_id",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_User_id",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_Group_id",
                table: "UserGroups");

            migrationBuilder.DropTable(
                name: "TestQuestion");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Creator_id",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResults_TestQuestion_id",
                table: "QuestionResults");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Creator_id",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Creator_id",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TestQuestion_id",
                table: "QuestionResults");

            migrationBuilder.DropColumn(
                name: "Creator_id",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UserGroup");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tests",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "TestQuestions_id",
                table: "QuestionResults",
                newName: "Question_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_User_id",
                table: "UserGroup",
                newName: "IX_UserGroup_User_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_Group_id",
                table: "UserGroup",
                newName: "IX_UserGroup_Group_id");

            migrationBuilder.AlterColumn<int>(
                name: "PassingMarks",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Marks",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Tests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Type",
                table: "Tests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Test_id",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Answer_time",
                table: "QuestionResults",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Semester",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroup",
                table: "UserGroup",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Test_id",
                table: "Questions",
                column: "Test_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResults_Question_id",
                table: "QuestionResults",
                column: "Question_id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionResults_Questions_Question_id",
                table: "QuestionResults",
                column: "Question_id",
                principalTable: "Questions",
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
                name: "FK_UserGroup_AspNetUsers_User_id",
                table: "UserGroup",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_Groups_Group_id",
                table: "UserGroup",
                column: "Group_id",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
