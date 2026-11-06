using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace casaDoBolo.Migrations
{
    /// <inheritdoc />
    public partial class HistoricoVendasDetalhado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorVendidoAno",
                table: "VendasAcumuladas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorVendidoAno",
                table: "VendasAcumuladas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
