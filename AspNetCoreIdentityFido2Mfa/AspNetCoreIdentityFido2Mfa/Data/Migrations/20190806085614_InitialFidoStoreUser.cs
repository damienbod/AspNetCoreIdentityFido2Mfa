using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreIdentityFido2Mfa.Data.Migrations
{
    public partial class InitialFidoStoreUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserId",
                table: "FidoStoredCredential",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "FidoStoredCredential",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "FidoStoredCredential");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserId",
                table: "FidoStoredCredential",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential",
                column: "UserId");
        }
    }
}
