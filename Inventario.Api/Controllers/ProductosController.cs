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
public class ProductosController : ControllerBase
{
    private readonly InventarioDbContext _context;

    public ProductosController(InventarioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
    {
        var productos = await _context.Productos
            .AsNoTracking()
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria != null
                    ? p.Categoria.Nombre
                    : null
            })
            .ToListAsync();

        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetProducto(int id)
    {
        var producto = await _context.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null)
        {
            return NotFound();
        }

        return Ok(new ProductoDto
        {
            Id = producto.Id,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Stock = producto.Stock,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria?.Nombre
        });
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> CrearProducto(
        CrearProductoRequest request)
    {
        if (request.CategoriaId.HasValue)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == request.CategoriaId.Value);

            if (!categoriaExiste)
            {
                return BadRequest("La categoría indicada no existe.");
            }
        }

        var producto = new Producto
        {
            Codigo = request.Codigo.Trim(),
            Nombre = request.Nombre.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Precio = request.Precio,
            Stock = request.Stock,
            CategoriaId = request.CategoriaId
        };

        _context.Productos.Add(producto);

        await _context.SaveChangesAsync();

        var categoriaNombre = request.CategoriaId.HasValue
            ? await _context.Categorias
                .Where(c => c.Id == request.CategoriaId.Value)
                .Select(c => c.Nombre)
                .FirstOrDefaultAsync()
            : null;

        var response = new ProductoDto
        {
            Id = producto.Id,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Stock = producto.Stock,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = categoriaNombre
        };

        return CreatedAtAction(
            nameof(GetProducto),
            new { id = producto.Id },
            response
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProducto(
        int id,
        ActualizarProductoRequest request)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null)
        {
            return NotFound();
        }

        if (request.CategoriaId.HasValue)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == request.CategoriaId.Value);

            if (!categoriaExiste)
            {
                return BadRequest("La categoría indicada no existe.");
            }
        }

        producto.Codigo = request.Codigo.Trim();
        producto.Nombre = request.Nombre.Trim();
        producto.Descripcion = request.Descripcion.Trim();
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.CategoriaId = request.CategoriaId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null)
        {
            return NotFound();
        }

        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}