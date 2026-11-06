using casaDoBolo.Data;
using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoEscolar.Controllers
{
    [Authorize(Roles = "Administrador(a)")]
    public class SaborController : Controller
    {
        private readonly IESContext _context;
        public SaborController(IESContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sabores.OrderBy(c => c.SaborID).ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sabor sabor, IFormFile FotoUpload)
        {
            if (ModelState.IsValid)
            {
                if (FotoUpload != null && FotoUpload.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await FotoUpload.CopyToAsync(ms);
                    sabor.Foto = ms.ToArray();
                }
                else
                {
                    // validação manual para obrigar foto
                    ModelState.AddModelError("Foto", "É necessário enviar uma foto.");
                    return View(sabor);
                }

                _context.Add(sabor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(sabor);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var sabor = await _context.Sabores.FindAsync(id);
            if (sabor == null) return NotFound();

            return View(sabor);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Sabor sabor, IFormFile FotoUpload)
        {
            if (id != sabor.SaborID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var saborExistente = await _context.Sabores.FindAsync(id);
                    if (saborExistente == null) return NotFound();

                    saborExistente.NomeSabor = sabor.NomeSabor;

                    if (FotoUpload != null && FotoUpload.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await FotoUpload.CopyToAsync(ms);
                        saborExistente.Foto = ms.ToArray();
                    }

                    _context.Update(saborExistente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    if (!_context.Sabores.Any(e => e.SaborID == sabor.SaborID))
                        return NotFound();
                    throw;
                }
            }
            return View(sabor);
        }

        // GET: Details
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var sabor = await _context.Sabores.Include(d => d.Pedidos)
                .FirstOrDefaultAsync(m => m.SaborID == id);
            if (sabor == null) return NotFound();

            return View(sabor);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var sabor = await _context.Sabores.FindAsync(id);
            if (sabor == null) return NotFound();

            return View(sabor);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var sabor = await _context.Sabores.FindAsync(id);
            _context.Sabores.Remove(sabor);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"Sabor {sabor.SaborID} foi removido";
            return RedirectToAction(nameof(Index));
        }
    }
}
