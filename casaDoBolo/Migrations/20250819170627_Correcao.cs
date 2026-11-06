using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace casaDoBolo.Migrations
{
    /// <inheritdoc />
    public partial class Correcao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_fk_ClienteID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Sabores_fk_SaborID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Tamanhos_fk_TamanhoID",
                table: "Pedidos");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Foto",
                table: "Sabores",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<long>(
                name: "fk_TamanhoID",
                table: "Pedidos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fk_SaborID",
                table: "Pedidos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fk_ClienteID",
                table: "Pedidos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_fk_ClienteID",
                table: "Pedidos",
                column: "fk_ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Sabores_fk_SaborID",
                table: "Pedidos",
                column: "fk_SaborID",
                principalTable: "Sabores",
                principalColumn: "SaborID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Tamanhos_fk_TamanhoID",
                table: "Pedidos",
                column: "fk_TamanhoID",
                principalTable: "Tamanhos",
                principalColumn: "TamanhoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_fk_ClienteID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Sabores_fk_SaborID",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Tamanhos_fk_TamanhoID",
                table: "Pedidos");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Foto",
                table: "Sabores",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "fk_TamanhoID",
                table: "Pedidos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "fk_SaborID",
                table: "Pedidos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "fk_ClienteID",
                table: "Pedidos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_fk_ClienteID",
                table: "Pedidos",
                column: "fk_ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Sabores_fk_SaborID",
                table: "Pedidos",
                column: "fk_SaborID",
                principalTable: "Sabores",
                principalColumn: "SaborID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Tamanhos_fk_TamanhoID",
                table: "Pedidos",
                column: "fk_TamanhoID",
                principalTable: "Tamanhos",
                principalColumn: "TamanhoID");
        }
    }
}
