using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarea1.Models.Dtos;
using Tarea1.Services;

namespace Tarea1.Controllers.Api
{
    /// <summary>API de gestión de usuarios (CRUD). Solo administradores.</summary>
    [ApiController]
    [Route("api/usuarios")]
    [Authorize(Roles = "Administrador")]
    public class UsuariosApiController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosApiController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _usuarioService.ListarAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _usuarioService.ObtenerAsync(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _usuarioService.CrearAsync(request);
            if (!result.Success) return BadRequest(new { mensaje = result.Error });

            return CreatedAtAction(nameof(Obtener), new { id = result.Data!.IdUsuario }, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioRequest request)
        {
            if (id != request.IdUsuario) return BadRequest(new { mensaje = "El Id no coincide." });
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _usuarioService.ActualizarAsync(request);
            if (!result.Success) return BadRequest(new { mensaje = result.Error });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _usuarioService.EliminarAsync(id);
            if (!result.Success) return BadRequest(new { mensaje = result.Error });

            return NoContent();
        }
    }
}
