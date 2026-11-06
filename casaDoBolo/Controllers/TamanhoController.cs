using casaDoBolo.Data;
using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoEscolar.Controllers
{
    [Authorize(Roles = "Administrador(a)")]
    public class TamanhoController : Controller
    {
        private readonly IESContext _context;
        public TamanhoController(IESContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tamanhos.OrderBy
                (c => c.TamanhoID).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Tamanho tamanho)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tamanho);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }     
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados.");
            }
            return View(tamanho);
        }
        //GET: Tamanho/Update
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tamanho = await _context.Tamanhos.SingleOrDefaultAsync(m => m.TamanhoID == id);

            if (tamanho == null)
            {
                return NotFound();
            }
            return View(tamanho);
        }

        private bool TamanhoExists(long? id)
        {
            return _context.Tamanhos.Any(e => e.TamanhoID == id);
        }

        //POST: Tamanho/Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Tamanho tamanho)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tamanho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!TamanhoExists(tamanho.TamanhoID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tamanho);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tamanho = await _context.Tamanhos.Include(d => d.Pedidos).
                SingleOrDefaultAsync(m => m.TamanhoID == id);

            if (tamanho == null)
            {
                return NotFound();
            }

            return View(tamanho);
        }

        //GET: Tamanho/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tamanho = await _context.Tamanhos
                .SingleOrDefaultAsync(d => d.TamanhoID == id);

            if (tamanho == null)
            {
                return NotFound();
            }
            return View(tamanho);
        }


        //POST: Tamanho/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var tamanho = await _context.Tamanhos.SingleOrDefaultAsync(m => m.TamanhoID == id);
            _context.Tamanhos.Remove(tamanho);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Tamanho {tamanho.TamanhoID} foi removido";

            return RedirectToAction(nameof(Index));
        }
    }
}
