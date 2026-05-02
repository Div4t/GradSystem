using GradSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace GradSystem.Services;

/// <summary>
/// Genera folios únicos con formato SIGLAS-YYYY-NNNNN.
/// El número de 5 dígitos es aleatorio; reintenta hasta obtener uno no duplicado.
/// </summary>
public class FolioService
{
    private readonly ApplicationDbContext _db;

    public FolioService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<string> GenerarFolioAsync(string universidadNombre)
    {
        // Obtener las siglas de la universidad seleccionada
        var siglas = await _db.Universidades
            .Where(u => u.Nombre == universidadNombre)
            .Select(u => u.Siglas)
            .FirstOrDefaultAsync()
            ?? "GRAD"; // fallback por si no se encuentra

        siglas = siglas.ToUpper().Trim();

        var anio = DateTime.UtcNow.Year;

        string folio;
        do
        {
            var num = Random.Shared.Next(1, 100_000); // 00001–99999
            folio = $"{siglas}-{anio}-{num:D5}";
        }
        while (await _db.Registros.AnyAsync(r => r.Folio == folio));

        return folio;
    }
}
