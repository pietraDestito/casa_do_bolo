using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using casaDoBolo.Data;

namespace casaDoBolo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IESContext _context;

        public HomeController(ILogger<HomeController> logger, IESContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var termoDeBusca = query.ToLower();

            var pedidos = _context.Pedidos
                                    .Include(p => p.Cliente)
                                    .Include(p => p.Sabor)
                                    .Include(p => p.Tamanho)
                                    .Where(p => p.FormaPagamento.ToLower().Contains(termoDeBusca) ||
                                                p.Cliente.Nome.ToLower().Contains(termoDeBusca) ||
                                                p.Sabor.NomeSabor.ToLower().Contains(termoDeBusca) ||
                                                p.Tamanho.Fatias.ToLower().Contains(termoDeBusca))
                                    .ToList();

            var clientes = _context.Clientes
                                    .Where(c => c.Nome.ToLower().Contains(termoDeBusca) ||
                                                c.CPF.ToLower().Contains(termoDeBusca) ||
                                                c.Telefone.ToLower().Contains(termoDeBusca) ||
                                                c.Email.ToLower().Contains(termoDeBusca) ||
                                                c.Endereco.ToLower().Contains(termoDeBusca))
                                    .ToList();

            var sabores = _context.Sabores
                                .Where(s => s.NomeSabor.ToLower().Contains(termoDeBusca))
                                .ToList();

            var tamanhos = _context.Tamanhos
                                .Where(t => t.Fatias.ToLower().Contains(termoDeBusca))
                                .ToList();

            ViewBag.SearchQuery = query;
            ViewBag.Pedidos = pedidos;
            ViewBag.Clientes = clientes;
            ViewBag.Sabores = sabores;
            ViewBag.Tamanhos = tamanhos;

            return View("Search");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}