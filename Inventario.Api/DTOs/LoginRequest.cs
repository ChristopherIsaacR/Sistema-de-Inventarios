using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.DTOs;

public class LoginRequest
{
    [Required]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}