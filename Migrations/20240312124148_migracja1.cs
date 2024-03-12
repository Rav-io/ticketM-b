using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketmanager.Migrations
{
    public partial class migracja1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => new { x.UserId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_UserTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ProjectName" },
                values: new object[,]
                {
                    { 1, "Project A" },
                    { 2, "Project B" },
                    { 3, "Project C" },
                    { 4, "Project D" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "ProjectId", "TaskDescription", "TaskName", "TaskStatus" },
                values: new object[,]
                {
                    { 1, "admin", new DateTime(2024, 3, 12, 13, 41, 48, 564, DateTimeKind.Local).AddTicks(6969), 1, "Description 1", "Task 1", 0 },
                    { 2, "admin", new DateTime(2024, 3, 12, 13, 41, 48, 564, DateTimeKind.Local).AddTicks(7011), 2, "Description 2", "Task 2", 1 },
                    { 3, "admin", new DateTime(2024, 3, 12, 13, 41, 48, 564, DateTimeKind.Local).AddTicks(7014), 3, "Description 3", "Task 3", 0 },
                    { 4, "admin", new DateTime(2024, 3, 12, 13, 41, 48, 564, DateTimeKind.Local).AddTicks(7016), 4, "Description 4", "Task 4", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "ProjectId", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 1, "$2b$10$qTsI5q5lJ4M/og7LSDuMcerNxY3qbkeWpEekYuOLE3F1juI0FMfI.", null, 1, "admin" },
                    { 2, "$2b$10$Q0euShiSntBJ8C7JwQ.koe4jZi2NFJ9luE3IzRx0yD16w9iNt9/NO", null, 1, "admin2" },
                    { 3, "$2b$10$ySROZX4JDwz.zsIuXtQVduoyKdp3ieuAPZlbCbBZtxwHqrluHmIKy", null, 2, "user" },
                    { 4, "$2b$10$1TufoEUl1Z6JumO9W7T5xOTNFkOWWWmz.QaOTLyBnJziOxz8OTuU2", null, 2, "user2" },
                    { 5, "$2b$10$o6IXLD48igXPJrRnB0mze.ptFGkpWGLeSeKqg7irPV42Y2raklfW.", null, 2, "user3" },
                    { 6, "$2b$10$G7xP5FjVXzp/n4qep.28feQUl5obVZSBEisrjCgmjgTsbk/D7xb7O", null, 2, "user4" },
                    { 7, "$2b$10$seOpQ9D7iGJDHyCbZoIhZ..J.XxAE4VLNpe5xuQyf7VSvVZ3Sgf/.", null, 2, "user5" },
                    { 8, "$2b$10$L9.ANlCIaTmZ/O1UbOQTFeN4VyYozIBWP.CJsNhbO.xe45Zwyszha", null, 2, "user6" },
                    { 9, "$2b$10$dbHa5ka/f2zOudo0aSeH9.v1rdlshVO0wxqZCOvjVD5SLnwhnUGhO", null, 2, "user7" }
                });

            migrationBuilder.InsertData(
                table: "UserProjects",
                columns: new[] { "ProjectId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 4, 3 },
                    { 3, 4 },
                    { 4, 4 },
                    { 3, 5 },
                    { 4, 5 },
                    { 2, 6 },
                    { 3, 6 },
                    { 3, 7 },
                    { 3, 8 },
                    { 3, 9 }
                });

            migrationBuilder.InsertData(
                table: "UserTasks",
                columns: new[] { "TaskId", "UserId" },
                values: new object[,]
                {
                    { 2, 4 },
                    { 3, 4 },
                    { 4, 4 },
                    { 3, 5 },
                    { 4, 6 },
                    { 3, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProjectId",
                table: "UserProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectId",
                table: "Users",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_TaskId",
                table: "UserTasks",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
