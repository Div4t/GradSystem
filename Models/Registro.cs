using System.ComponentModel.DataAnnotations;

namespace GradSystem.Models;

public class Registro
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Folio { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Display(Name = "Universidad")]
    public string Universidad { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Display(Name = "Carrera")]
    public string Carrera { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Display(Name = "Nombre(s)")]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Display(Name = "Apellido Paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Display(Name = "Apellido Materno")]
    public string ApellidoMaterno { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Display(Name = "Número de Fotografía")]
    public string NumeroFotografia { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [Display(Name = "Dedicatoria 1")]
    public string Dedicatoria1 { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Dedicatoria 2")]
    public string? Dedicatoria2 { get; set; }

    [MaxLength(500)]
    [Display(Name = "Dedicatoria 3")]
    public string? Dedicatoria3 { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Color del Jersey")]
    public string ColorJersey { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Display(Name = "Talla")]
    public string Talla { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Display(Name = "Abreviatura")]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Display(Name = "Nombre en la Espalda")]
    public string NombreEspalda { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [Display(Name = "Número en la Espalda")]
    public string NumeroEspalda { get; set; } = string.Empty;

    [Display(Name = "Fecha de Envío")]
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

    [Display(Name = "Última Edición")]
    public DateTime? UltimaEdicion { get; set; }

    [MaxLength(200)]
    [Display(Name = "Editado Por")]
    public string? EditadoPor { get; set; }

    // Propiedad calculada para nombre completo
    public string NombreCompleto => $"{Nombres} {ApellidoPaterno} {ApellidoMaterno}";
}
