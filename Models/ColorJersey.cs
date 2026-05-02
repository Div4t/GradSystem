using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

/// <summary>
/// Catálogo de colores de jersey, configurable desde el panel admin.
/// </summary>
public class ColorJersey
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Color")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Orden")]
    public int Orden { get; set; } = 0;
}
