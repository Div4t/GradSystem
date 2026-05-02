using GradSystem.Data;
using GradSystem.Models;
using GradSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GradSystem.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly SignInManager<IdentityUser> _signIn;
    private readonly UserManager<IdentityUser> _userManager;

    private const int TamanioPagina = 20;

    public AdminController(
        ApplicationDbContext db,
        SignInManager<IdentityUser> signIn,
        UserManager<IdentityUser> userManager)
    {
        _db = db;
        _signIn = signIn;
        _userManager = userManager;
    }

    // ── Módulo 3: Autenticación ───────────────────────────────────────────

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Index));

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password,
                                           bool recordar = false,
                                           string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Ingresa tu correo y contraseña.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var resultado = await _signIn.PasswordSignInAsync(
            email, password, isPersistent: recordar, lockoutOnFailure: true);

        if (resultado.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        if (resultado.IsLockedOut)
            ViewBag.Error = "Cuenta bloqueada temporalmente. Intenta más tarde.";
        else
            ViewBag.Error = "Correo o contraseña incorrectos.";

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    // ── Módulo 4: Panel de administración ────────────────────────────────

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(
        string? busqueda,
        string? filtroColor,
        string? filtroTalla,
        string? filtroUniversidad,
        int pagina = 1)
    {
        var query = _db.Registros.AsQueryable();

        // Búsqueda por texto libre
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var b = busqueda.Trim().ToLower();
            query = query.Where(r =>
                r.Folio.ToLower().Contains(b) ||
                r.Nombres.ToLower().Contains(b) ||
                r.ApellidoPaterno.ToLower().Contains(b) ||
                r.ApellidoMaterno.ToLower().Contains(b) ||
                r.Universidad.ToLower().Contains(b) ||
                r.Carrera.ToLower().Contains(b));
        }

        // Filtros
        if (!string.IsNullOrWhiteSpace(filtroColor))
            query = query.Where(r => r.ColorJersey == filtroColor);

        if (!string.IsNullOrWhiteSpace(filtroTalla))
            query = query.Where(r => r.Talla == filtroTalla);

        if (!string.IsNullOrWhiteSpace(filtroUniversidad))
            query = query.Where(r => r.Universidad == filtroUniversidad);

        var total = await query.CountAsync();
        var totalPaginas = (int)Math.Ceiling(total / (double)TamanioPagina);
        pagina = Math.Max(1, Math.Min(pagina, Math.Max(1, totalPaginas)));

        var registros = await query
            .OrderByDescending(r => r.FechaEnvio)
            .Skip((pagina - 1) * TamanioPagina)
            .Take(TamanioPagina)
            .ToListAsync();

        var config = await _db.ConfiguracionSistema.FindAsync(1);

        var vm = new AdminListaViewModel
        {
            Registros            = registros,
            PaginaActual         = pagina,
            TotalPaginas         = Math.Max(1, totalPaginas),
            TotalRegistros       = total,
            TamanioPagina        = TamanioPagina,
            Busqueda             = busqueda,
            FiltroColor          = filtroColor,
            FiltroTalla          = filtroTalla,
            FiltroUniversidad    = filtroUniversidad,
            RegistroHabilitado   = config?.RegistroHabilitado ?? true,
            ColoresDisponibles   = await _db.ColoresJersey
                                       .Where(c => c.Activo)
                                       .OrderBy(c => c.Orden)
                                       .Select(c => c.Nombre)
                                       .ToListAsync(),
            UniversidadesDisponibles = await _db.Registros
                                           .Select(r => r.Universidad)
                                           .Distinct()
                                           .OrderBy(u => u)
                                           .ToListAsync(),
        };

        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var registro = await _db.Registros.FindAsync(id);
        if (registro is null) return NotFound();
        return View(registro);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _db.Registros.FindAsync(id);
        if (registro is null) return NotFound();

        var vm = MapearAViewModel(registro);
        await RecargarListas(vm);
        return View(vm);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EditarRegistroViewModel vm)
    {
        if (id != vm.Id)
            return BadRequest();

        if (vm.DedicatoriasSeleccionadas == null || vm.DedicatoriasSeleccionadas.Count == 0)
            ModelState.AddModelError("DedicatoriasSeleccionadas", "Selecciona al menos una dedicatoria.");
        else if (vm.DedicatoriasSeleccionadas.Count > 3)
            ModelState.AddModelError("DedicatoriasSeleccionadas", "Puedes seleccionar máximo 3 dedicatorias.");

        if (!ModelState.IsValid)
        {
            await RecargarListas(vm);
            return View(vm);
        }

        var registro = await _db.Registros.FindAsync(id);
        if (registro is null) return NotFound();

        registro.Universidad      = vm.Universidad;
        registro.Carrera          = vm.Carrera;
        registro.Nombres          = vm.Nombres;
        registro.ApellidoPaterno  = vm.ApellidoPaterno;
        registro.ApellidoMaterno  = vm.ApellidoMaterno;
        registro.NumeroFotografia = vm.NumeroFotografia;
        registro.Dedicatoria1     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(0) ?? string.Empty;
        registro.Dedicatoria2     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(1);
        registro.Dedicatoria3     = vm.DedicatoriasSeleccionadas.ElementAtOrDefault(2);
        registro.ColorJersey      = vm.ColorJersey;
        registro.Talla            = vm.Talla;
        registro.Titulo           = vm.Titulo;
        registro.NombreEspalda    = vm.NombreEspalda;
        registro.NumeroEspalda    = vm.NumeroEspalda;
        registro.UltimaEdicion    = DateTime.UtcNow;
        registro.EditadoPor       = User.Identity?.Name ?? "admin";

        await _db.SaveChangesAsync();

        TempData["Mensaje"] = $"Registro {registro.Folio} actualizado correctamente.";
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleRegistro()
    {
        var config = await _db.ConfiguracionSistema.FindAsync(1);
        if (config is null)
        {
            config = new Models.ConfiguracionSistema { Id = 1, RegistroHabilitado = false };
            _db.ConfiguracionSistema.Add(config);
        }
        else
        {
            config.RegistroHabilitado = !config.RegistroHabilitado;
        }
        await _db.SaveChangesAsync();

        TempData["Mensaje"] = config.RegistroHabilitado
            ? "El registro ha sido habilitado correctamente."
            : "El registro ha sido deshabilitado correctamente.";

        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static EditarRegistroViewModel MapearAViewModel(Registro r)
    {
        var dedicatorias = new List<string>();
        if (!string.IsNullOrEmpty(r.Dedicatoria1)) dedicatorias.Add(r.Dedicatoria1);
        if (!string.IsNullOrEmpty(r.Dedicatoria2)) dedicatorias.Add(r.Dedicatoria2);
        if (!string.IsNullOrEmpty(r.Dedicatoria3)) dedicatorias.Add(r.Dedicatoria3);

        return new EditarRegistroViewModel
        {
            Id                        = r.Id,
            Folio                     = r.Folio,
            Universidad               = r.Universidad,
            Carrera                   = r.Carrera,
            Nombres                   = r.Nombres,
            ApellidoPaterno           = r.ApellidoPaterno,
            ApellidoMaterno           = r.ApellidoMaterno,
            NumeroFotografia          = r.NumeroFotografia,
            DedicatoriasSeleccionadas = dedicatorias,
            ColorJersey               = r.ColorJersey,
            Talla                     = r.Talla,
            Titulo                    = r.Titulo,
            NombreEspalda             = r.NombreEspalda,
            NumeroEspalda             = r.NumeroEspalda,
        };
    }

    private async Task RecargarListas(EditarRegistroViewModel vm)
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
