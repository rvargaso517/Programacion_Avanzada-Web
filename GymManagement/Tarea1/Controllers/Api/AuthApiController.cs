using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarea1.Models.Dtos;
using Tarea1.Services;

namespace Tarea1.Controllers.Api
{
    /// <summary>API de autenticación: login, registro y recuperación de contraseña.</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Inicia sesión y devuelve un JWT.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _authService.LoginAsync(request);
            if (!result.Success) return Unauthorized(new { mensaje = result.Error });

            return Ok(result.Data);
        }

        /// <summary>Registra un nuevo usuario (auto-registro).</summary>
        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _authService.RegistrarAsync(request);
            if (!result.Success) return BadRequest(new { mensaje = result.Error });

            return Ok(result.Data);
        }

        /// <summary>Solicita un token de recuperación de contraseña.</summary>
        [HttpPost("recuperar")]
        [AllowAnonymous]
        public async Task<IActionResult> Recuperar([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _authService.SolicitarRecuperacionAsync(request);
            // Respuesta genérica: no se revela si el correo existe.
            return Ok(new
            {
                mensaje = "Si el correo está registrado, se enviaron las instrucciones de recuperación.",
                token = result.Data // en producción se enviaría por correo, no en la respuesta.
            });
        }

        /// <summary>Restablece la contraseña usando un token válido.</summary>
        [HttpPost("restablecer")]
        [AllowAnonymous]
        public async Task<IActionResult> Restablecer([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _authService.RestablecerPasswordAsync(request);
            if (!result.Success) return BadRequest(new { mensaje = result.Error });

            return Ok(new { mensaje = "La contraseña se actualizó correctamente." });
        }

        /// <summary>Devuelve los datos del usuario autenticado (requiere JWT).</summary>
        [HttpGet("perfil")]
        [Authorize]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                nombre = User.Identity?.Name,
                correo = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
