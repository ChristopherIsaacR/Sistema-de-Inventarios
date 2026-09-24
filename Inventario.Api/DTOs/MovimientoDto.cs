using Inventario.Api.Models;

namespace Inventario.Api.DTOs;

public class MovimientoDto
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public string ProductoCodigo { get; set; } = string.Empty;

    public string ProductoNombre { get; set; } = string.Empty;

    public TipoMovimiento Tipo { get; set; }

    public int Cantidad { get; set; }

    public int StockAnterior { get; set; }

    public int StockResultante { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }
}