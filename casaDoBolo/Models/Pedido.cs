using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace casaDoBolo.Models
{
    public class Pedido
    {
        [Key]
        public long PedidoID { get; set; }  

        [Required(ErrorMessage = "Informe a data de retirada")]
        [Display(Name = "Data de Retirada")]
        public DateTime DataRetirada { get; set; }

        [Required(ErrorMessage = "Informe a forma de pagamento")]
        [Display(Name = "Forma de Pagamento")]
        public string? FormaPagamento { get; set; }

        

        [Required(ErrorMessage = "O valor total do pedido é obrigatório.")]
        [Column(TypeName = "decimal(18, 2)")] 
        [Display(Name = "Valor Total")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Entregue")]
        public bool Entregue { get; set; } = false; 

        [Display(Name = "Data de Entrega")]
        public DateTime? DataEntrega { get; set; } 


        [Required(ErrorMessage = "Selecione o cliente")]
        [Display(Name = "Cliente")]
        public long fk_ClienteID { get; set; }
        [ForeignKey("fk_ClienteID")]
        public Cliente? Cliente { get; set; }


        [Required(ErrorMessage = "Selecione o sabor")]
        [Display(Name = "Sabor")]
        public long fk_SaborID { get; set; }
        [ForeignKey("fk_SaborID")]
        public Sabor? Sabor { get; set; }


        [Required(ErrorMessage = "Selecione o tamanho")]
        [Display(Name = "Tamanho")]
        public long fk_TamanhoID { get; set; }
        [ForeignKey("fk_TamanhoID")]
        public Tamanho? Tamanho { get; set; }
    }
}