using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgilerapProcessSystems.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderID = table.Column<int>(type: "int", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateByID = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateByID = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Work_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Status",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Work_User_CreateByID",
                        column: x => x.CreateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Work_User_UpdateByID",
                        column: x => x.UpdateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreateByID = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateByID = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Provider_User_CreateByID",
                        column: x => x.CreateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Provider_User_UpdateByID",
                        column: x => x.UpdateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Provider_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Provider_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WorkLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkID = table.Column<int>(type: "int", nullable: true),
                    No = table.Column<int>(type: "int", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateByID = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateByID = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkLog_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Status",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WorkLog_User_CreateByID",
                        column: x => x.CreateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WorkLog_User_UpdateByID",
                        column: x => x.UpdateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WorkLog_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProviderLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkLogID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreateByID = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateByID = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProviderLog_User_CreateByID",
                        column: x => x.CreateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProviderLog_User_UpdateByID",
                        column: x => x.UpdateByID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProviderLog_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProviderLog_WorkLog_WorkLogID",
                        column: x => x.WorkLogID,
                        principalTable: "WorkLog",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_CreateByID",
                table: "Provider",
                column: "CreateByID");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_UpdateByID",
                table: "Provider",
                column: "UpdateByID");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_UserID",
                table: "Provider",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_WorkID",
                table: "Provider",
                column: "WorkID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLog_CreateByID",
                table: "ProviderLog",
                column: "CreateByID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLog_UpdateByID",
                table: "ProviderLog",
                column: "UpdateByID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLog_UserID",
                table: "ProviderLog",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLog_WorkLogID",
                table: "ProviderLog",
                column: "WorkLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_CreateByID",
                table: "Work",
                column: "CreateByID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_StatusID",
                table: "Work",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Work_UpdateByID",
                table: "Work",
                column: "UpdateByID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLog_CreateByID",
                table: "WorkLog",
                column: "CreateByID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLog_StatusID",
                table: "WorkLog",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLog_UpdateByID",
                table: "WorkLog",
                column: "UpdateByID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLog_WorkID",
                table: "WorkLog",
                column: "WorkID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "ProviderLog");

            migrationBuilder.DropTable(
                name: "WorkLog");

            migrationBuilder.DropTable(
                name: "Work");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
