namespace casaDoBolo.Models
{
    public class HistoricoMensalDetalhado
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public decimal ValorVendidoMes { get; set; }

        public List<Pedido> PedidosEntregues { get; set; }
    }
}