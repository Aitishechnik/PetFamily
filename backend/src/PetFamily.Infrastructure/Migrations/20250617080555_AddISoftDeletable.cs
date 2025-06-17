using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddISoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pets_volonteers_volonteer_id",
                table: "pets");

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "volonteers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                table: "volonteers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "pets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                table: "pets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_pets_volonteers_volonteer_id",
                table: "pets",
                column: "volonteer_id",
                principalTable: "volonteers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pets_volonteers_volonteer_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "deleted",
                table: "volonteers");

            migrationBuilder.DropColumn(
                name: "deletion_date",
                table: "volonteers");

            migrationBuilder.DropColumn(
                name: "deleted",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "deletion_date",
                table: "pets");

            migrationBuilder.AddForeignKey(
                name: "fk_pets_volonteers_volonteer_id",
                table: "pets",
                column: "volonteer_id",
                principalTable: "volonteers",
                principalColumn: "id");
        }
    }
}
