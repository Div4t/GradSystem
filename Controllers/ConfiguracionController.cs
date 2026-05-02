using GradSystem.Data;
using GradSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradSystem.Controllers;

[Authorize]
public class ConfiguracionController : Controller
{
    private readonly ApplicationDbContext _db;

    public ConfiguracionController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string tab = "universidades")
    {
        ViewBag.Tab           = tab;
        ViewBag.Universidades = await _db.Universidades.OrderBy(u => u.Orden).ToListAsync();
        ViewBag.Colores       = await _db.ColoresJersey.OrderBy(c => c.Orden).ToListAsync();
        ViewBag.Tallas        = await _db.Tallas.OrderBy(t => t.Orden).ToListAsync();
        ViewBag.Dedicatorias  = await _db.Dedicatorias.OrderBy(d => d.Orden).ToListAsync();
        ViewBag.Titulos       = await _db.Titulos.OrderBy(t => t.Orden).ToListAsync();
        return View();
    }

    // ── Universidades ─────────────────────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarUniversidad(string nombre, string siglas)
    {
        if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(siglas))
        {
            var maxOrden = await _db.Universidades.AnyAsync()
                ? await _db.Universidades.MaxAsync(u => u.Orden) : 0;

            _db.Universidades.Add(new UniversidadCatalogo
            {
                Nombre = nombre.Trim(),
                Siglas = siglas.Trim().ToUpper(),
                Activo = true,
                Orden  = maxOrden + 1
            });
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Universidad agregada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "universidades" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarUniversidad(int id, string nombre, string siglas, bool activo, int orden)
    {
        var item = await _db.Universidades.FindAsync(id);
        if (item is not null)
        {
            item.Nombre  = nombre.Trim();
            item.Siglas  = siglas.Trim().ToUpper();
            item.Activo  = activo;
            item.Orden   = orden;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Universidad actualizada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "universidades" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarUniversidad(int id)
    {
        var item = await _db.Universidades.FindAsync(id);
        if (item is not null)
        {
            _db.Universidades.Remove(item);
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Universidad eliminada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "universidades" });
    }

    // ── Colores de Jersey ─────────────────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarColor(string nombre)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var maxOrden = await _db.ColoresJersey.AnyAsync()
                ? await _db.ColoresJersey.MaxAsync(c => c.Orden) : 0;

            _db.ColoresJersey.Add(new ColorJersey
            {
                Nombre = nombre.Trim(),
                Activo = true,
                Orden  = maxOrden + 1
            });
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Color agregado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "colores" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarColor(int id, string nombre, bool activo, int orden)
    {
        var item = await _db.ColoresJersey.FindAsync(id);
        if (item is not null)
        {
            item.Nombre = nombre.Trim();
            item.Activo = activo;
            item.Orden  = orden;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Color actualizado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "colores" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarColor(int id)
    {
        var item = await _db.ColoresJersey.FindAsync(id);
        if (item is not null)
        {
            _db.ColoresJersey.Remove(item);
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Color eliminado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "colores" });
    }

    // ── Dedicatorias ──────────────────────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarDedicatoria(string nombre)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var maxOrden = await _db.Dedicatorias.AnyAsync()
                ? await _db.Dedicatorias.MaxAsync(d => d.Orden) : 0;

            _db.Dedicatorias.Add(new DedicatoriaCatalogo
            {
                Nombre = nombre.Trim(),
                Activo = true,
                Orden  = maxOrden + 1
            });
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Dedicatoria agregada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "dedicatorias" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarDedicatoria(int id, string nombre, bool activo, int orden)
    {
        var item = await _db.Dedicatorias.FindAsync(id);
        if (item is not null)
        {
            item.Nombre = nombre.Trim();
            item.Activo = activo;
            item.Orden  = orden;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Dedicatoria actualizada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "dedicatorias" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarDedicatoria(int id)
    {
        var item = await _db.Dedicatorias.FindAsync(id);
        if (item is not null)
        {
            _db.Dedicatorias.Remove(item);
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Dedicatoria eliminada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "dedicatorias" });
    }

    // ── Títulos ───────────────────────────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarTitulo(string nombre)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var maxOrden = await _db.Titulos.AnyAsync()
                ? await _db.Titulos.MaxAsync(t => t.Orden) : 0;
            _db.Titulos.Add(new TituloCatalogo { Nombre = nombre.Trim(), Activo = true, Orden = maxOrden + 1 });
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Título agregado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "titulos" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarTitulo(int id, string nombre, bool activo, int orden)
    {
        var item = await _db.Titulos.FindAsync(id);
        if (item is not null)
        {
            item.Nombre = nombre.Trim();
            item.Activo = activo;
            item.Orden  = orden;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Título actualizado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "titulos" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarTitulo(int id)
    {
        var item = await _db.Titulos.FindAsync(id);
        if (item is not null)
        {
            _db.Titulos.Remove(item);
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Título eliminado.";
        }
        return RedirectToAction(nameof(Index), new { tab = "titulos" });
    }

    // ── Tallas ────────────────────────────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarTalla(string nombre)
    {
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var maxOrden = await _db.Tallas.AnyAsync()
                ? await _db.Tallas.MaxAsync(t => t.Orden) : 0;

            _db.Tallas.Add(new TallaCatalogo
            {
                Nombre = nombre.Trim(),
                Activo = true,
                Orden  = maxOrden + 1
            });
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Talla agregada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "tallas" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarTalla(int id, string nombre, bool activo, int orden)
    {
        var item = await _db.Tallas.FindAsync(id);
        if (item is not null)
        {
            item.Nombre = nombre.Trim();
            item.Activo = activo;
            item.Orden  = orden;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Talla actualizada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "tallas" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarTalla(int id)
    {
        var item = await _db.Tallas.FindAsync(id);
        if (item is not null)
        {
            _db.Tallas.Remove(item);
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "Talla eliminada.";
        }
        return RedirectToAction(nameof(Index), new { tab = "tallas" });
    }
}
