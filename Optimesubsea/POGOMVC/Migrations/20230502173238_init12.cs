using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POGOMVC.Migrations
{
    public partial class init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_PasscodeRecoveryQuestionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionNarration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_PasscodeRecoveryQuestionnaire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_FileUploadModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C1 = table.Column<int>(type: "int", nullable: false),
                    C2 = table.Column<int>(type: "int", nullable: false),
                    C3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    C4 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_FileUploadModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_UserRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoleIdId = table.Column<int>(type: "int", nullable: true),
                    PasscodeRecoveryQuestionnaireId1Id = table.Column<int>(type: "int", nullable: true),
                    PasscodeRecoveryAnswer1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_UserRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_UserRegistration_m_PasscodeRecoveryQuestionnaire_PasscodeRecoveryQuestionnaireId1Id",
                        column: x => x.PasscodeRecoveryQuestionnaireId1Id,
                        principalTable: "m_PasscodeRecoveryQuestionnaire",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_UserRegistration_m_Roles_UserRoleIdId",
                        column: x => x.UserRoleIdId,
                        principalTable: "m_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_ProjectTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuperUserId = table.Column<int>(type: "int", nullable: true),
                    UploadDataEndUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_ProjectTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_ProjectTable_t_UserRegistration_SuperUserId",
                        column: x => x.SuperUserId,
                        principalTable: "t_UserRegistration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_ProjectTable_t_UserRegistration_UploadDataEndUserId",
                        column: x => x.UploadDataEndUserId,
                        principalTable: "t_UserRegistration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_UserHasProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRegistrationId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_UserHasProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_UserHasProjects_t_ProjectTable_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "t_ProjectTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_t_UserHasProjects_t_UserRegistration_UserRegistrationId",
                        column: x => x.UserRegistrationId,
                        principalTable: "t_UserRegistration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_ProjectTable_SuperUserId",
                table: "t_ProjectTable",
                column: "SuperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_t_ProjectTable_UploadDataEndUserId",
                table: "t_ProjectTable",
                column: "UploadDataEndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_t_UserHasProjects_ProjectId",
                table: "t_UserHasProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_t_UserHasProjects_UserRegistrationId",
                table: "t_UserHasProjects",
                column: "UserRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_t_UserRegistration_PasscodeRecoveryQuestionnaireId1Id",
                table: "t_UserRegistration",
                column: "PasscodeRecoveryQuestionnaireId1Id");

            migrationBuilder.CreateIndex(
                name: "IX_t_UserRegistration_UserRoleIdId",
                table: "t_UserRegistration",
                column: "UserRoleIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_FileUploadModels");

            migrationBuilder.DropTable(
                name: "t_UserHasProjects");

            migrationBuilder.DropTable(
                name: "t_ProjectTable");

            migrationBuilder.DropTable(
                name: "t_UserRegistration");

            migrationBuilder.DropTable(
                name: "m_PasscodeRecoveryQuestionnaire");

            migrationBuilder.DropTable(
                name: "m_Roles");
        }
    }
}
