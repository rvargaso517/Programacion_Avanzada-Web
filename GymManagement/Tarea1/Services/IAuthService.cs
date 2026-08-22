using Tarea1.Models;
using Tarea1.Models.Dtos;

namespace Tarea1.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request);
        Task<ServiceResult<UsuarioDto>> RegistrarAsync(RegistroRequest request);

        /// <summary>Genera un token de recuperación. Data contiene el token si el correo existe.</summary>
        Task<ServiceResult<string?>> SolicitarRecuperacionAsync(ForgotPasswordRequest request);

        Task<ServiceResult<bool>> RestablecerPasswordAsync(ResetPasswordRequest request);
    }
}
