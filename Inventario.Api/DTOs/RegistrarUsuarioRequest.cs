using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.DTOs;

public class RegistrarUsuarioRequest
{
    [Required]
    [StringLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}