using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

public class TituloCatalogo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Abreviatura")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Display(Name = "Orden")]
    public int Orden { get; set; } = 0;
}
