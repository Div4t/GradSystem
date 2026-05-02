using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

public class UniversidadCatalogo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    [Display(Name = "Universidad")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Display(Name = "Siglas")]
    public string Siglas { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Orden")]
    public int Orden { get; set; } = 0;
}
