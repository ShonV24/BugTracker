using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class Initialize_fixed_Schema_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId1",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_OwnerUserId1",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_DeveloperUserId1",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_OwnerUserId1",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketPriorityId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketStatusId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketTypeId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "DeveloperUserId1",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "OwnerUserId1",
                table: "Ticket");

            migrationBuilder.AlterColumn<string>(
                name: "TicketTypeId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TicketStatusId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TicketPriorityId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerUserId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DeveloperUserId",
                table: "Ticket",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "TicketPriorityId1",
                table: "Ticket",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketStatusId1",
                table: "Ticket",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketTypeId1",
                table: "Ticket",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_DeveloperUserId",
                table: "Ticket",
                column: "DeveloperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OwnerUserId",
                table: "Ticket",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketPriorityId1",
                table: "Ticket",
                column: "TicketPriorityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketStatusId1",
                table: "Ticket",
                column: "TicketStatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketTypeId1",
                table: "Ticket",
                column: "TicketTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId",
                table: "Ticket",
                column: "DeveloperUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_OwnerUserId",
                table: "Ticket",
                column: "OwnerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId1",
                table: "Ticket",
                column: "TicketPriorityId1",
                principalTable: "TicketPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId1",
                table: "Ticket",
                column: "TicketStatusId1",
                principalTable: "TicketStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId1",
                table: "Ticket",
                column: "TicketTypeId1",
                principalTable: "TicketType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_OwnerUserId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId1",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId1",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId1",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_DeveloperUserId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_OwnerUserId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketPriorityId1",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketStatusId1",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TicketTypeId1",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketPriorityId1",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketStatusId1",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketTypeId1",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "TicketTypeId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TicketStatusId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TicketPriorityId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerUserId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperUserId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeveloperUserId1",
                table: "Ticket",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId1",
                table: "Ticket",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_DeveloperUserId1",
                table: "Ticket",
                column: "DeveloperUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OwnerUserId1",
                table: "Ticket",
                column: "OwnerUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketPriorityId",
                table: "Ticket",
                column: "TicketPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketStatusId",
                table: "Ticket",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_DeveloperUserId1",
                table: "Ticket",
                column: "DeveloperUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_OwnerUserId1",
                table: "Ticket",
                column: "OwnerUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketPriority_TicketPriorityId",
                table: "Ticket",
                column: "TicketPriorityId",
                principalTable: "TicketPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_TicketStatusId",
                table: "Ticket",
                column: "TicketStatusId",
                principalTable: "TicketStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketType_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId",
                principalTable: "TicketType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
