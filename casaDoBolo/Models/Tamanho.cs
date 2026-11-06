using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace casaDoBolo.Models
{
    public class Tamanho
    {
        [Key]
        public long TamanhoID { get; set; }

        [Required(ErrorMessage = "Informe a quantidade de fatias.")]
        public string Fatias { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o valor.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        public IList<Pedido>? Pedidos { get; set; }
    }
}
