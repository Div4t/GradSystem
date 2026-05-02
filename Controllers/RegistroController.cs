using GradSystem.Data;
using GradSystem.Models;
using GradSystem.Models.ViewModels;
using GradSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GradSystem.Controllers;

public class RegistroController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly FolioService _folioService;

    public RegistroController(ApplicationDbContext db, FolioService folioService)
    {
        _db = db;
        _folioService = folioService;
    }

    // ── Módulo 1: Formulario público ──────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Formulario()
    {
        var config = await _db.ConfiguracionSistema.FindAsync(1);
        if (config?.RegistroHabilitado == false)
        {
            ViewBag.RegistroDeshabilitado = true;
            return View(new RegistroFormViewModel());
        }

        return View(await ConstruirViewModelAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Formulario(RegistroFormViewModel vm)
    {
        var config = await _db.ConfiguracionSistema.FindAsync(1);
        if (config?.RegistroHabilitado == false)
        {
            ViewBag.RegistroDeshabilitado = true;
            return View(new RegistroFormViewModel());
        }

        // Validación dedicatorias: mínimo 1, máximo 3
        if (vm.DedicatoriasSeleccionadas == null || vm.DedicatoriasSeleccionadas.Count == 0)
            ModelState.AddModelError("DedicatoriasSeleccionadas", "Selecciona al menos una dedicatoria.");
        else if (vm.DedicatoriasSeleccionadas.Count > 3)
            ModelState.AddModelError("DedicatoriasSeleccionadas", "Puedes seleccionar máximo 3 dedicatorias.");

        if (!ModelState.IsValid)
        {
            await RecargarListas(vm);
            return View(vm);
        }

        var folio = await _folioService.GenerarFolioAsync(vm.Universidad);

        var registro = new Registro
        {
            Folio            = folio,
            Universidad      = vm.Universidad,
            Carrera          = vm.Carrera,
            Nombres          = vm.Nombres,
            ApellidoPaterno  = vm.ApellidoPaterno,
            ApellidoMaterno  = vm.ApellidoMaterno,
            NumeroFotografia = vm.NumeroFotografia,
            Dedicatoria1     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(0) ?? string.Empty,
            Dedicatoria2     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(1),
            Dedicatoria3     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(2),
            ColorJersey      = vm.ColorJersey,
            Talla            = vm.Talla,
            Titulo           = vm.Titulo,
            NombreEspalda    = vm.NombreEspalda,
            NumeroEspalda    = vm.NumeroEspalda,
            FechaEnvio       = DateTime.UtcNow,
        };

        _db.Registros.Add(registro);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Confirmacion), new { folio });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmacion(string folio)
    {
        var registro = await _db.Registros.FirstOrDefaultAsync(r => r.Folio == folio);
        if (registro is null) return NotFound();
        return View(registro);
    }

    // ── Módulo 2: Consulta por folio ──────────────────────────────────────

    [HttpGet]
    public IActionResult Consulta() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Consulta(string folio)
    {
        if (string.IsNullOrWhiteSpace(folio))
        {
            ModelState.AddModelError("folio", "Ingresa un folio válido.");
            return View();
        }

        var registro = await _db.Registros
            .FirstOrDefaultAsync(r => r.Folio == folio.Trim().ToUpper());

        if (registro is null)
        {
            ViewBag.FolioNoEncontrado = folio;
            return View();
        }

        return RedirectToAction(nameof(Detalle), new { folio = registro.Folio });
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(string folio)
    {
        var registro = await _db.Registros.FirstOrDefaultAsync(r => r.Folio == folio);
        if (registro is null) return NotFound();
        return View(registro);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private async Task<RegistroFormViewModel> ConstruirViewModelAsync()
    {
        var vm = new RegistroFormViewModel();
        await RecargarListas(vm);
        return vm;
    }

    private async Task RecargarListas(RegistroFormViewModel vm)
    {
        vm.ColoresDisponibles = await _db.ColoresJersey
            .Where(c => c.Activo).OrderBy(c => c.Orden)
            .Select(c => new SelectListItem(c.Nombre, c.Nombre))
            .ToListAsync();

        vm.UniversidadesDisponibles = await _db.Universidades
            .Where(u => u.Activo).OrderBy(u => u.Orden)
            .Select(u => new SelectListItem(u.Nombre, u.Nombre))
            .ToListAsync();

        vm.TallasDisponibles = await _db.Tallas
            .Where(t => t.Activo).OrderBy(t => t.Orden)
            .Select(t => new SelectListItem(t.Nombre, t.Nombre))
            .ToListAsync();

        vm.DedicatoriasDisponibles = await _db.Dedicatorias
            .Where(d => d.Activo).OrderBy(d => d.Orden)
            .Select(d => new SelectListItem(d.Nombre, d.Nombre))
            .ToListAsync();

        vm.TitulosDisponibles = await _db.Titulos
            .Where(t => t.Activo).OrderBy(t => t.Orden)
            .Select(t => new SelectListItem(t.Nombre, t.Nombre))
            .ToListAsync();
    }
}
