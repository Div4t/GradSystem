using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GradSystem.Models.ViewModels;

public class EditarRegistroViewModel
{
    public int Id { get; set; }

    [Display(Name = "Folio")]
    public string Folio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La universidad es requerida.")]
    [MaxLength(200)]
    [Display(Name = "Universidad")]
    public string Universidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La carrera es requerida.")]
    [MaxLength(200)]
    [Display(Name = "Carrera")]
    public string Carrera { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(200)]
    [Display(Name = "Nombre(s)")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido paterno es requerido.")]
    [MaxLength(100)]
    [Display(Name = "Apellido Paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido materno es requerido.")]
    [MaxLength(100)]
    [Display(Name = "Apellido Materno")]
    public string ApellidoMaterno { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de fotografía es requerido.")]
    [MaxLength(50)]
    [Display(Name = "Número de Fotografía")]
    public string NumeroFotografia { get; set; } = string.Empty;

    // Dedicatorias: selección múltiple (1 a 3 opciones)
    [Display(Name = "Dedicatorias")]
    public List<string> DedicatoriasSeleccionadas { get; set; } = new();

    [Required(ErrorMessage = "El color del jersey es requerido.")]
    [MaxLength(100)]
    [Display(Name = "Color del Jersey")]
    public string ColorJersey { get; set; } = string.Empty;

    [Required(ErrorMessage = "La talla es requerida.")]
    [MaxLength(10)]
    [Display(Name = "Talla")]
    public string Talla { get; set; } = string.Empty;

    [Required(ErrorMessage = "El título es requerido.")]
    [MaxLength(50)]
    [Display(Name = "Abreviatura")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre en la espalda es requerido.")]
    [MaxLength(200)]
    [Display(Name = "Nombre en la Espalda")]
    public string NombreEspalda { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número en la espalda es requerido.")]
    [MaxLength(2)]
    [Display(Name = "Número en la Espalda")]
    public string NumeroEspalda { get; set; } = string.Empty;

    // Listas cargadas desde la BD
    public List<SelectListItem> ColoresDisponibles        { get; set; } = new();
    public List<SelectListItem> UniversidadesDisponibles  { get; set; } = new();
    public List<SelectListItem> TallasDisponibles         { get; set; } = new();
    public List<SelectListItem> DedicatoriasDisponibles   { get; set; } = new();
    public List<SelectListItem> TitulosDisponibles        { get; set; } = new();
}
