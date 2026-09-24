using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Inventario.Api.Data;
using Inventario.Api.DTOs;
using Inventario.Api.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Inventario.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly InventarioDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthController(
        InventarioDbContext context,
        IPasswordHasher<Usuario> passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(
        RegistrarUsuarioRequest request)
    {
        var nombreUsuario = request.NombreUsuario.Trim();

        var existe = await _context.Usuarios
            .AnyAsync(u => u.NombreUsuario == nombreUsuario);

        if (existe)
        {
            return Conflict("El usuario ya existe.");
        }

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario
        };

        usuario.PasswordHash = _passwordHasher.HashPassword(
            usuario,
            request.Password
        );

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensaje = "Usuario creado correctamente.",
            usuario = usuario.NombreUsuario
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(
                u => u.NombreUsuario == request.NombreUsuario
            );

        if (usuario is null)
        {
            return Unauthorized(
                "Usuario o contraseña incorrectos."
            );
        }

        var resultado =
            _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                request.Password
            );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return Unauthorized(
                "Usuario o contraseña incorrectos."
            );
        }

        var token = GenerarToken(usuario);

        return Ok(new LoginResponse
        {
            Token = token,
            Usuario = usuario.NombreUsuario
        });
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = _configuration["JwtSettings:Key"]!;
        var issuer = _configuration["JwtSettings:Issuer"]!;
        var audience = _configuration["JwtSettings:Audience"]!;

        var expirationMinutes = int.Parse(
            _configuration["JwtSettings:ExpirationMinutes"] ?? "60"
        );

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()
            ),

            new Claim(
                ClaimTypes.Name,
                usuario.NombreUsuario
            )
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expirationMinutes
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}