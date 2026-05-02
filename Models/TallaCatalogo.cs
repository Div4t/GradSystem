using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

public class TallaCatalogo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [Display(Name = "Talla")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Orden")]
    public int Orden { get; set; } = 0;
}
