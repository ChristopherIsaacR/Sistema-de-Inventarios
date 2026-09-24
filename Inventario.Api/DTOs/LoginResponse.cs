namespace Inventario.Api.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;
}