using System.ComponentModel.DataAnnotations.Schema;

namespace casaDoBolo.Models
{
    public class VendasAcumuladas
    {
        public int VendasAcumuladasID { get; set; }


        public int Ano { get; set; }
        public int Mes { get; set; } 


        [Column(TypeName = "decimal(18, 2)")] 
        public decimal ValorVendidoMes { get; set; }

        
    }
}