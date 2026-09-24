using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.DTOs;

public class CrearProductoRequest
{
    [Required]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public int? CategoriaId { get; set; }
}