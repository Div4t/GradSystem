using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

public class DedicatoriaCatalogo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    [Display(Name = "Dedicatoria")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Orden")]
    public int Orden { get; set; } = 0;
}
