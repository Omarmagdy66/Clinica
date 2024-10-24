using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cls.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Patients_PatientId",
                table: "Queries");

            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Users_UserId",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_PatientId",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_UserId",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Queries");

            migrationBuilder.AlterColumn<DateTime>(
                name: "QueryDate",
                table: "Queries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Queries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Queries");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "QueryDate",
                table: "Queries",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Queries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Queries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Queries_PatientId",
                table: "Queries",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Queries_UserId",
                table: "Queries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Patients_PatientId",
                table: "Queries",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Users_UserId",
                table: "Queries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
