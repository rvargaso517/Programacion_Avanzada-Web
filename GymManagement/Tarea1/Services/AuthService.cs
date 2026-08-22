using Tarea1.Models;
using Tarea1.Models.Dtos;
using Tarea1.Repositories;
using Tarea1.Security;

namespace Tarea1.Services
{
    public class AuthService : IAuthService
    {
        /// <summary>Rol asignado por defecto al auto-registro público.</summary>
        private const string RolPorDefecto = "Recepcionista";
        private const int MinutosVigenciaToken = 60;

        private readonly IUsuarioRepository _usuarios;
        private readonly IRolRepository _roles;
        private readonly IRecuperacionRepository _recuperaciones;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(
            IUsuarioRepository usuarios,
            IRolRepository roles,
            IRecuperacionRepository recuperaciones,
            IPasswordHasher hasher,
            IJwtTokenGenerator jwt)
        {
            _usuarios = usuarios;
            _roles = roles;
            _recuperaciones = recuperaciones;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarios.ObtenerPorCorreoAsync(request.Correo.Trim());
            if (usuario is null || !_hasher.Verify(request.Password, usuario.PasswordHash))
                return ServiceResult<AuthResponse>.Fail("Correo o contraseña incorrectos.");

            if (!usuario.Estado)
                return ServiceResult<AuthResponse>.Fail("El usuario está inactivo. Contacte al administrador.");

            var (token, expira) = _jwt.GenerateToken(usuario);

            var response = new AuthResponse
            {
                Token = token,
                Expira = expira,
                Usuario = Map(usuario)
            };
            return ServiceResult<AuthResponse>.Ok(response);
        }

        public async Task<ServiceResult<UsuarioDto>> RegistrarAsync(RegistroRequest request)
        {
            var correo = request.Correo.Trim();

            if (await _usuarios.ExisteCorreoAsync(correo))
                return ServiceResult<UsuarioDto>.Fail("Ya existe un usuario con ese correo.");

            var roles = await _roles.ListarAsync();
            var rol = roles.FirstOrDefault(r =>
                r.Nombre.Equals(RolPorDefecto, StringComparison.OrdinalIgnoreCase));
            if (rol is null)
                return ServiceResult<UsuarioDto>.Fail("No se encontró el rol por defecto para el registro.");

            var hash = _hasher.Hash(request.Password);
            var id = await _usuarios.CrearAsync(rol.IdRol, request.Nombre.Trim(), correo, hash, true);

            var creado = await _usuarios.ObtenerPorIdAsync(id);
            return ServiceResult<UsuarioDto>.Ok(Map(creado!));
        }

        public async Task<ServiceResult<string?>> SolicitarRecuperacionAsync(ForgotPasswordRequest request)
        {
            var usuario = await _usuarios.ObtenerPorCorreoAsync(request.Correo.Trim());

            // Por seguridad no se revela si el correo existe: siempre "éxito".
            if (usuario is null)
                return ServiceResult<string?>.Ok(null);

            var token = Guid.NewGuid().ToString("N");
            var expira = DateTime.Now.AddMinutes(MinutosVigenciaToken);
            await _recuperaciones.CrearAsync(usuario.IdUsuario, token, expira);

            // El token se devuelve para que el controlador arme el enlace / lo envíe por correo.
            return ServiceResult<string?>.Ok(token);
        }

        public async Task<ServiceResult<bool>> RestablecerPasswordAsync(ResetPasswordRequest request)
        {
            var registro = await _recuperaciones.ObtenerPorTokenAsync(request.Token);
            if (registro is null)
                return ServiceResult<bool>.Fail("El enlace de recuperación no es válido.");

            if (registro.Utilizado)
                return ServiceResult<bool>.Fail("Este enlace de recuperación ya fue utilizado.");

            if (registro.FechaExpira < DateTime.Now)
                return ServiceResult<bool>.Fail("El enlace de recuperación ha expirado.");

            var hash = _hasher.Hash(request.NuevaPassword);
            await _usuarios.ActualizarPasswordAsync(registro.IdUsuario, hash);
            await _recuperaciones.MarcarUtilizadoAsync(registro.IdRecuperacion);

            return ServiceResult<bool>.Ok(true);
        }

        private static UsuarioDto Map(Models.Entities.Usuario u) => new()
        {
            IdUsuario = u.IdUsuario,
            IdRol = u.IdRol,
            RolNombre = u.RolNombre ?? string.Empty,
            Nombre = u.Nombre,
            Correo = u.Correo,
            Estado = u.Estado,
            FechaRegistro = u.FechaRegistro
        };
    }
}
