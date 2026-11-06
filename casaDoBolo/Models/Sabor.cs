using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace casaDoBolo.Models
{
    public class Sabor
    {
        [Key]
        public long SaborID { get; set; }

        [Required(ErrorMessage = "Informe o nome do sabor.")]
        public string NomeSabor { get; set; } = string.Empty;

        public byte[]? Foto { get; set; }

        public IList<Pedido>? Pedidos { get; set; }
    }
}
