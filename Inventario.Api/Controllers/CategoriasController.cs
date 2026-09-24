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
public class CategoriasController : ControllerBase
{
    private readonly InventarioDbContext _context;

    public CategoriasController(InventarioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
        {
            return NotFound();
        }

        return Ok(new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        });
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> CrearCategoria(
        CrearCategoriaRequest request)
    {
        var nombre = request.Nombre.Trim();

        var existe = await _context.Categorias
            .AnyAsync(c => c.Nombre == nombre);

        if (existe)
        {
            return Conflict("La categoría ya existe.");
        }

        var categoria = new Categoria
        {
            Nombre = nombre,
            Descripcion = request.Descripcion.Trim()
        };

        _context.Categorias.Add(categoria);

        await _context.SaveChangesAsync();

        var response = new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };

        return CreatedAtAction(
            nameof(GetCategoria),
            new { id = categoria.Id },
            response
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
        {
            return NotFound();
        }

        _context.Categorias.Remove(categoria);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}