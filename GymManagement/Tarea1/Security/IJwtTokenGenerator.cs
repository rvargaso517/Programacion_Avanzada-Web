using Tarea1.Models.Entities;

namespace Tarea1.Security
{
    /// <summary>Genera tokens JWT para los usuarios autenticados.</summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>Genera un JWT y devuelve el token junto con su fecha de expiración.</summary>
        (string token, DateTime expira) GenerateToken(Usuario usuario);
    }
}
