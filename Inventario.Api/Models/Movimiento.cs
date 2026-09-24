namespace Inventario.Api.Models;

public class Movimiento
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public Producto Producto { get; set; } = null!;

    public TipoMovimiento Tipo { get; set; }

    public int Cantidad { get; set; }

    public int StockAnterior { get; set; }

    public int StockResultante { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }
}