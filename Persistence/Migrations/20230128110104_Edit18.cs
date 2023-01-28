using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Edit18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorSub_FactorHeader_FactorHeaderID",
                table: "FactorSub");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorSub_Products_ProductID",
                table: "FactorSub");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorSub",
                table: "FactorSub");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorHeader",
                table: "FactorHeader");

            migrationBuilder.RenameTable(
                name: "FactorSub",
                newName: "FactorSubs");

            migrationBuilder.RenameTable(
                name: "FactorHeader",
                newName: "FactorHeaders");

            migrationBuilder.RenameIndex(
                name: "IX_FactorSub_ProductID",
                table: "FactorSubs",
                newName: "IX_FactorSubs_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_FactorSub_FactorHeaderID",
                table: "FactorSubs",
                newName: "IX_FactorSubs_FactorHeaderID");

            migrationBuilder.AlterColumn<int>(
                name: "DisCount",
                table: "FactorSubs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorSubs",
                table: "FactorSubs",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorHeaders",
                table: "FactorHeaders",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorSubs_FactorHeaders_FactorHeaderID",
                table: "FactorSubs",
                column: "FactorHeaderID",
                principalTable: "FactorHeaders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorSubs_Products_ProductID",
                table: "FactorSubs",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorSubs_FactorHeaders_FactorHeaderID",
                table: "FactorSubs");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorSubs_Products_ProductID",
                table: "FactorSubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorSubs",
                table: "FactorSubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorHeaders",
                table: "FactorHeaders");

            migrationBuilder.RenameTable(
                name: "FactorSubs",
                newName: "FactorSub");

            migrationBuilder.RenameTable(
                name: "FactorHeaders",
                newName: "FactorHeader");

            migrationBuilder.RenameIndex(
                name: "IX_FactorSubs_ProductID",
                table: "FactorSub",
                newName: "IX_FactorSub_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_FactorSubs_FactorHeaderID",
                table: "FactorSub",
                newName: "IX_FactorSub_FactorHeaderID");

            migrationBuilder.AlterColumn<string>(
                name: "DisCount",
                table: "FactorSub",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorSub",
                table: "FactorSub",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorHeader",
                table: "FactorHeader",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorSub_FactorHeader_FactorHeaderID",
                table: "FactorSub",
                column: "FactorHeaderID",
                principalTable: "FactorHeader",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorSub_Products_ProductID",
                table: "FactorSub",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
