using System.ComponentModel.DataAnnotations;

using Inventario.Api.Models;

namespace Inventario.Api.DTOs;

public class CrearMovimientoRequest
{
    [Range(1, int.MaxValue)]
    public int ProductoId { get; set; }

    [Required]
    public TipoMovimiento Tipo { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [StringLength(250)]
    public string Motivo { get; set; } = string.Empty;
}