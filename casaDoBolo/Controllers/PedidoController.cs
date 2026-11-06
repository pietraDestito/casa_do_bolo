using casaDoBolo.Data;
using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace casaDoBolo.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IESContext _context;

        public PedidoController(IESContext context)
        {
            _context = context;
        }

        // GET: Pedido/Index
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Sabor)
                .Include(p => p.Tamanho)
                .OrderByDescending(p => p.PedidoID)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: Pedido/Create
        public IActionResult Create()
        {
            CarregarDropdowns();
            return View();
        }

        // POST: Pedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            // Validação de Horário de Funcionamento no Back-end
            if (!ValidarHorarioFuncionamento(pedido.DataRetirada))
            {
                ModelState.AddModelError("DataRetirada", "A data/hora selecionada está fora do nosso expediente de funcionamento (Seg-Sex: 8h às 18h | Sáb-Dom: 8h às 12h).");
            }

            if (ModelState.IsValid)
            {
                var tamanho = await _context.Tamanhos.FindAsync(pedido.fk_TamanhoID);

                if (tamanho != null)
                {
                    pedido.ValorTotal = tamanho.Valor;
                }
                else
                {
                    ModelState.AddModelError("", "Tamanho inválido. Não foi possível atribuir o preço.");
                    CarregarDropdowns(pedido);
                    return View(pedido);
                }

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Pedido criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns(pedido);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente).Include(p => p.Sabor).Include(p => p.Tamanho)
                .FirstOrDefaultAsync(p => p.PedidoID == id);

            if (pedido == null) return NotFound();

            CarregarDropdowns(pedido);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Pedido pedido)
        {
            if (id != pedido.PedidoID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var tamanho = await _context.Tamanhos.FindAsync(pedido.fk_TamanhoID);

                    if (tamanho != null)
                    {
                        pedido.ValorTotal = tamanho.Valor;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tamanho inválido. Não foi possível atribuir o preço.");
                        CarregarDropdowns(pedido);
                        return View(pedido);
                    }

                    var originalPedido = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(p => p.PedidoID == id);
                    if (originalPedido != null)
                    {
                        pedido.Entregue = originalPedido.Entregue;
                        pedido.DataEntrega = originalPedido.DataEntrega;
                    }

                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Pedido atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pedidos.Any(e => e.PedidoID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns(pedido);
            return View(pedido);
        }

        // GET: Pedido/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente).Include(p => p.Sabor).Include(p => p.Tamanho)
                .FirstOrDefaultAsync(p => p.PedidoID == id);

            if (pedido == null) return NotFound();
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente).Include(p => p.Sabor).Include(p => p.Tamanho)
                .FirstOrDefaultAsync(p => p.PedidoID == id);

            if (pedido == null) return NotFound();
            return View(pedido);
        }

        // POST: Pedido/Delete/5 (Executa a exclusão)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Pedido excluído com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível excluir o pedido. Pode haver registros relacionados (dependências) que precisam ser excluídos primeiro.");

                var pedidoComDetalhes = await _context.Pedidos
                    .Include(p => p.Cliente).Include(p => p.Sabor).Include(p => p.Tamanho)
                    .FirstOrDefaultAsync(p => p.PedidoID == id);

                return View(pedidoComDetalhes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarEntregue(long id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Tamanho)
                .FirstOrDefaultAsync(p => p.PedidoID == id);

            if (pedido == null)
            {
                return NotFound();
            }

            if (pedido.Entregue)
            {
                return Ok();
            }

            decimal valorPedido = pedido.ValorTotal > 0 ? pedido.ValorTotal :
                                 (pedido.Tamanho != null ? pedido.Tamanho.Valor : 0);

            pedido.Entregue = true;
            pedido.DataEntrega = DateTime.Now;

            if (valorPedido > 0)
            {
                await AtualizarVendasAcumuladas(pedido, valorPedido);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Entrega marcada e vendas contabilizadas com sucesso!";
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Falha ao processar a entrega: " + ex.Message);
            }

            return RedirectToAction(nameof(Details), new { id = pedido.PedidoID });
        }

        private async Task AtualizarVendasAcumuladas(Pedido pedido, decimal valorVendido)
        {
            if (valorVendido <= 0 || pedido.DataRetirada == DateTime.MinValue) return;

            var ano = pedido.DataRetirada.Year;
            var mes = pedido.DataRetirada.Month;

            var vendas = await _context.VendasAcumuladas
                .FirstOrDefaultAsync(v => v.Ano == ano && v.Mes == mes);

            if (vendas == null)
            {
                vendas = new VendasAcumuladas
                {
                    Ano = ano,
                    Mes = mes,
                    ValorVendidoMes = 0,
                };
                _context.VendasAcumuladas.Add(vendas);
            }

            vendas.ValorVendidoMes += valorVendido;
        }

        // Método auxiliar de validação de horário
        private bool ValidarHorarioFuncionamento(DateTime dataRetirada)
        {
            var diaSemana = dataRetirada.DayOfWeek;
            var hora = dataRetirada.Hour;
            var minuto = dataRetirada.Minute;
            var horaDecimal = hora + (minuto / 60.0);

            // Segunda a Sexta (Segunda = 1, Sexta = 5)
            if (diaSemana >= DayOfWeek.Monday && diaSemana <= DayOfWeek.Friday)
            {
                return horaDecimal >= 8 && horaDecimal < 18;
            }
            // Sábado (6) ou Domingo (0)
            else if (diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday)
            {
                return horaDecimal >= 8 && horaDecimal < 12;
            }

            return false;
        }

        private void CarregarDropdowns(Pedido pedido = null)
        {
            ViewBag.Clientes = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "ClienteID",
                "Nome",
                pedido?.fk_ClienteID
            );

            ViewBag.Sabores = new SelectList(
                _context.Sabores.OrderBy(s => s.NomeSabor),
                "SaborID",
                "NomeSabor",
                pedido?.fk_SaborID
            );

            ViewBag.Tamanhos = new SelectList(
                _context.Tamanhos.OrderBy(t => t.Fatias),
                "TamanhoID",
                "Fatias",
                pedido?.fk_TamanhoID
            );
        }
    }
}