#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatViewer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChatEventTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO ChatEventType (ChatEventName)
            VALUES 
                ('enter-the-room'),
                ('leave-the-room'),
                ('comment'),
                ('high-five-another-user')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE ChatEventType");
        }
    }
}
