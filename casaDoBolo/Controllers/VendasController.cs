using casaDoBolo.Data;
using casaDoBolo.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace casaDoBolo.Controllers
{
    [Authorize]
    public class VendasController : Controller
    {
        private readonly IESContext _context;

        public VendasController(IESContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Historico()
        {
            

            var pedidosEntregues = await _context.Set<Pedido>()
                .Include(p => p.Cliente)
                .Include(p => p.Sabor)
                .Include(p => p.Tamanho) 
                .Where(p => p.Entregue && p.DataEntrega.HasValue) 
                .ToListAsync();


            var historicoDetalhado = pedidosEntregues

                .GroupBy(p => new { Ano = p.DataEntrega.Value.Year, Mes = p.DataEntrega.Value.Month })
                .Select(g => new HistoricoMensalDetalhado 
                {
                    Ano = g.Key.Ano,
                    Mes = g.Key.Mes,

                    ValorVendidoMes = g.Sum(p => p.Tamanho.Valor),


                    PedidosEntregues = g.OrderBy(p => p.DataEntrega).ToList()
                })
                .OrderByDescending(h => h.Ano)
                .ThenByDescending(h => h.Mes)
                .ToList();


            return View(historicoDetalhado);
        }
    }
}