using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreIdentityFido2Mfa.Data.Migrations
{
    public partial class Allow_Multiple_keys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "FidoStoredCredential",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FidoStoredCredential",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FidoStoredCredential");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "FidoStoredCredential",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FidoStoredCredential",
                table: "FidoStoredCredential",
                column: "Username");
        }
    }
}
