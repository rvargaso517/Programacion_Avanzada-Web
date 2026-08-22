using Tarea1.Models;
using Tarea1.Models.Dtos;
using Tarea1.Models.Entities;
using Tarea1.Repositories;
using Tarea1.Security;

namespace Tarea1.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarios;
        private readonly IRolRepository _roles;
        private readonly IPasswordHasher _hasher;

        public UsuarioService(
            IUsuarioRepository usuarios,
            IRolRepository roles,
            IPasswordHasher hasher)
        {
            _usuarios = usuarios;
            _roles = roles;
            _hasher = hasher;
        }

        public async Task<IEnumerable<UsuarioDto>> ListarAsync()
        {
            var usuarios = await _usuarios.ListarAsync();
            return usuarios.Select(Map);
        }

        public async Task<UsuarioDto?> ObtenerAsync(int idUsuario)
        {
            var usuario = await _usuarios.ObtenerPorIdAsync(idUsuario);
            return usuario is null ? null : Map(usuario);
        }

        public Task<IEnumerable<Rol>> ListarRolesAsync() => _roles.ListarAsync();

        public async Task<ServiceResult<UsuarioDto>> CrearAsync(CrearUsuarioRequest request)
        {
            var correo = request.Correo.Trim();

            if (await _usuarios.ExisteCorreoAsync(correo))
                return ServiceResult<UsuarioDto>.Fail("Ya existe un usuario con ese correo.");

            var hash = _hasher.Hash(request.Password);
            var id = await _usuarios.CrearAsync(request.IdRol, request.Nombre.Trim(), correo, hash, request.Estado);

            var creado = await _usuarios.ObtenerPorIdAsync(id);
            return ServiceResult<UsuarioDto>.Ok(Map(creado!));
        }

        public async Task<ServiceResult<bool>> ActualizarAsync(ActualizarUsuarioRequest request)
        {
            var existente = await _usuarios.ObtenerPorIdAsync(request.IdUsuario);
            if (existente is null)
                return ServiceResult<bool>.Fail("El usuario no existe.");

            var correo = request.Correo.Trim();
            if (await _usuarios.ExisteCorreoAsync(correo, request.IdUsuario))
                return ServiceResult<bool>.Fail("Ya existe otro usuario con ese correo.");

            await _usuarios.ActualizarAsync(request.IdUsuario, request.IdRol, request.Nombre.Trim(), correo, request.Estado);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> EliminarAsync(int idUsuario)
        {
            var existente = await _usuarios.ObtenerPorIdAsync(idUsuario);
            if (existente is null)
                return ServiceResult<bool>.Fail("El usuario no existe.");

            await _usuarios.EliminarAsync(idUsuario);
            return ServiceResult<bool>.Ok(true);
        }

        private static UsuarioDto Map(Usuario u) => new()
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
