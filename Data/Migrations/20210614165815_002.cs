using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Invite",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Invite_InviteeId",
                table: "Invite",
                column: "InviteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_InvitorId",
                table: "Invite",
                column: "InvitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_ProjectId",
                table: "Invite",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_AspNetUsers_InviteeId",
                table: "Invite",
                column: "InviteeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_AspNetUsers_InvitorId",
                table: "Invite",
                column: "InvitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invite_Project_ProjectId",
                table: "Invite",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invite_AspNetUsers_InviteeId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_AspNetUsers_InvitorId",
                table: "Invite");

            migrationBuilder.DropForeignKey(
                name: "FK_Invite_Project_ProjectId",
                table: "Invite");

            migrationBuilder.DropIndex(
                name: "IX_Invite_InviteeId",
                table: "Invite");

            migrationBuilder.DropIndex(
                name: "IX_Invite_InvitorId",
                table: "Invite");

            migrationBuilder.DropIndex(
                name: "IX_Invite_ProjectId",
                table: "Invite");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Invite");
        }
    }
}
