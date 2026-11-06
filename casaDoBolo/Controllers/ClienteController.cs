using casaDoBolo.Data;
using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoEscolar.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IESContext _context;
        public ClienteController(IESContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.OrderBy
                (c => c.ClienteID).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados.");
            }
            return View(cliente);
        }
        //GET: Cliente/Update
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Clientes.SingleOrDefaultAsync(m => m.ClienteID == id);

            if (instituicao == null)
            {
                return NotFound();
            }
            return View(instituicao);
        }

        private bool ClienteExists(long? id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }

        //POST: Cliente/Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!ClienteExists(cliente.ClienteID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.Include(d => d.Pedidos).
                SingleOrDefaultAsync(m => m.ClienteID == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        //GET: Cliente/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .SingleOrDefaultAsync(d => d.ClienteID == id);

            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }


        //POST: Cliente/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var cliente = await _context.Clientes.SingleOrDefaultAsync(m => m.ClienteID == id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Cliente {cliente.ClienteID} foi removido";

            return RedirectToAction(nameof(Index));
        }
    }
}
