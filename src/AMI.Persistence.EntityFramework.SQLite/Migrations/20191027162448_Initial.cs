using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMI.Persistence.EntityFramework.SQLite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    SubEventType = table.Column<int>(nullable: false),
                    EventSerialized = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ApiVersion = table.Column<string>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    EventSerialized = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    DataType = table.Column<int>(nullable: false),
                    FileFormat = table.Column<int>(nullable: false),
                    OriginalFilename = table.Column<string>(nullable: false),
                    SourcePath = table.Column<string>(nullable: false),
                    ExtractedPath = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    BasePath = table.Column<string>(nullable: true),
                    JsonFilename = table.Column<string>(nullable: true),
                    ResultType = table.Column<int>(nullable: false),
                    ResultSerialized = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(maxLength: 64, nullable: false),
                    NormalizedUsername = table.Column<string>(maxLength: 64, nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 128, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Roles = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Webhooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(maxLength: 2048, nullable: false),
                    ApiVersion = table.Column<string>(maxLength: 128, nullable: true),
                    Secret = table.Column<string>(maxLength: 4096, nullable: false),
                    EnabledEvents = table.Column<string>(maxLength: 4096, nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webhooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    QueuedDate = table.Column<DateTime>(nullable: true),
                    StartedDate = table.Column<DateTime>(nullable: true),
                    EndedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Progress = table.Column<int>(nullable: false),
                    CommandType = table.Column<int>(nullable: false),
                    CommandSerialized = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    ObjectId = table.Column<Guid>(nullable: true),
                    ResultId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Objects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastUsedDate = table.Column<DateTime>(nullable: false),
                    TokenValue = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEvents_Timestamp",
                table: "AuditEvents",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedDate",
                table: "Events",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventType",
                table: "Events",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedDate_UserId",
                table: "Events",
                columns: new[] { "CreatedDate", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedDate_EventType_UserId",
                table: "Events",
                columns: new[] { "CreatedDate", "EventType", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Objects_CreatedDate",
                table: "Objects",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_UserId",
                table: "Objects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedDate",
                table: "Tasks",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ResultId",
                table: "Tasks",
                column: "ResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ObjectId_Status",
                table: "Tasks",
                columns: new[] { "ObjectId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_CreatedDate",
                table: "Tokens",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_LastUsedDate",
                table: "Tokens",
                column: "LastUsedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedDate",
                table: "Users",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUsername",
                table: "Users",
                column: "NormalizedUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_CreatedDate",
                table: "Webhooks",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_EnabledEvents",
                table: "Webhooks",
                column: "EnabledEvents");

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_UserId",
                table: "Webhooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_UserId_EnabledEvents",
                table: "Webhooks",
                columns: new[] { "UserId", "EnabledEvents" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Webhooks");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
