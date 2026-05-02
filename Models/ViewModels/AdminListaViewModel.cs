using GradSystem.Models;

namespace GradSystem.Models.ViewModels;

public class AdminListaViewModel
{
    // Registros paginados
    public List<Registro> Registros { get; set; } = new();

    // Paginación
    public int PaginaActual  { get; set; } = 1;
    public int TotalPaginas  { get; set; } = 1;
    public int TotalRegistros { get; set; } = 0;
    public int TamanioPagina { get; set; } = 20;

    // Parámetros de búsqueda/filtro
    public string? Busqueda    { get; set; }
    public string? FiltroColor { get; set; }
    public string? FiltroTalla { get; set; }
    public string? FiltroUniversidad { get; set; }

    // Opciones disponibles para los filtros
    public List<string> ColoresDisponibles       { get; set; } = new();
    public List<string> UniversidadesDisponibles { get; set; } = new();

    public static readonly List<string> TallasDisponibles =
        new() { "XS", "S", "M", "L", "XL", "XXL" };

    public bool HayPaginaAnterior => PaginaActual > 1;
    public bool HayPaginaSiguiente => PaginaActual < TotalPaginas;
}
