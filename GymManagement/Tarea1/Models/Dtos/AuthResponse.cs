namespace Tarea1.Models.Dtos
{
    /// <summary>Resultado de una autenticación exitosa: token JWT + datos del usuario.</summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira { get; set; }
        public UsuarioDto Usuario { get; set; } = new();
    }
}
