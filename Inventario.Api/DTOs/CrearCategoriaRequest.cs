using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.DTOs;

public class CrearCategoriaRequest
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;
}