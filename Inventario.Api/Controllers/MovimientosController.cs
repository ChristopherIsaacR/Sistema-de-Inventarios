using Inventario.Api.Data;
using Inventario.Api.DTOs;
using Inventario.Api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly InventarioDbContext _context;

    public MovimientosController(InventarioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientoDto>>> GetMovimientos()
    {
        var movimientos = await _context.Movimientos
            .AsNoTracking()
            .OrderByDescending(m => m.Fecha)
            .Select(m => new MovimientoDto
            {
                Id = m.Id,
                ProductoId = m.ProductoId,
                ProductoCodigo = m.Producto.Codigo,
                ProductoNombre = m.Producto.Nombre,
                Tipo = m.Tipo,
                Cantidad = m.Cantidad,
                StockAnterior = m.StockAnterior,
                StockResultante = m.StockResultante,
                Motivo = m.Motivo,
                Fecha = m.Fecha
            })
            .ToListAsync();

        return Ok(movimientos);
    }

    [HttpPost]
    public async Task<ActionResult<MovimientoDto>> CrearMovimiento(
        CrearMovimientoRequest request)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == request.ProductoId);

        if (producto is null)
        {
            return NotFound("El producto no existe.");
        }

        if (!Enum.IsDefined(request.Tipo))
        {
            return BadRequest("El tipo de movimiento no es válido.");
        }

        var stockAnterior = producto.Stock;

        if (request.Tipo == TipoMovimiento.Salida)
        {
            if (producto.Stock < request.Cantidad)
            {
                return BadRequest(
                    "No hay stock suficiente para realizar la salida."
                );
            }

            producto.Stock -= request.Cantidad;
        }
        else
        {
            producto.Stock += request.Cantidad;
        }

        var movimiento = new Movimiento
        {
            ProductoId = producto.Id,
            Tipo = request.Tipo,
            Cantidad = request.Cantidad,
            StockAnterior = stockAnterior,
            StockResultante = producto.Stock,
            Motivo = request.Motivo.Trim(),
            Fecha = DateTime.UtcNow
        };

        _context.Movimientos.Add(movimiento);

        await _context.SaveChangesAsync();

        var response = new MovimientoDto
        {
            Id = movimiento.Id,
            ProductoId = producto.Id,
            ProductoCodigo = producto.Codigo,
            ProductoNombre = producto.Nombre,
            Tipo = movimiento.Tipo,
            Cantidad = movimiento.Cantidad,
            StockAnterior = movimiento.StockAnterior,
            StockResultante = movimiento.StockResultante,
            Motivo = movimiento.Motivo,
            Fecha = movimiento.Fecha
        };

        return Ok(response);
    }
}