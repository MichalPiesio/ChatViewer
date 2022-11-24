#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatViewer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatEventType",
                columns: table => new
                {
                    ChatEventName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEventType", x => x.ChatEventName);
                });

            migrationBuilder.CreateTable(
                name: "Chatter",
                columns: table => new
                {
                    ChatterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatter", x => x.ChatterId);
                });

            migrationBuilder.CreateTable(
                name: "ChatEvent",
                columns: table => new
                {
                    ChatEventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChatEventName = table.Column<string>(type: "TEXT", nullable: true),
                    ChatterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Chatter2Id = table.Column<Guid>(type: "TEXT", nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEvent", x => x.ChatEventId);
                    table.ForeignKey(
                        name: "FK_ChatEvent_ChatEventType_ChatEventName",
                        column: x => x.ChatEventName,
                        principalTable: "ChatEventType",
                        principalColumn: "ChatEventName");
                    table.ForeignKey(
                        name: "FK_ChatEvent_Chatter_Chatter2Id",
                        column: x => x.Chatter2Id,
                        principalTable: "Chatter",
                        principalColumn: "ChatterId");
                    table.ForeignKey(
                        name: "FK_ChatEvent_Chatter_ChatterId",
                        column: x => x.ChatterId,
                        principalTable: "Chatter",
                        principalColumn: "ChatterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatEvent_ChatEventName",
                table: "ChatEvent",
                column: "ChatEventName");

            migrationBuilder.CreateIndex(
                name: "IX_ChatEvent_Chatter2Id",
                table: "ChatEvent",
                column: "Chatter2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatEvent_ChatterId",
                table: "ChatEvent",
                column: "ChatterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatEvent");

            migrationBuilder.DropTable(
                name: "ChatEventType");

            migrationBuilder.DropTable(
                name: "Chatter");
        }
    }
}
