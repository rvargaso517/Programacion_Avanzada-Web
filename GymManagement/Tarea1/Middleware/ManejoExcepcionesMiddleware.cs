using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Tarea1.Repositories;

namespace Tarea1.Middleware
{
    /// <summary>
    /// Captura cualquier excepción no controlada de la aplicación, la guarda en
    /// dbo.LogErrores y devuelve una respuesta amigable al usuario.
    /// Se registra de primero en la tubería (ver Program.cs).
    /// </summary>
    public class ManejoExcepcionesMiddleware
    {
        private readonly RequestDelegate _siguiente;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ManejoExcepcionesMiddleware> _logger;

        public ManejoExcepcionesMiddleware(
            RequestDelegate siguiente,
            IServiceScopeFactory scopeFactory,
            ILogger<ManejoExcepcionesMiddleware> logger)
        {
            _siguiente = siguiente;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _siguiente(context);
            }
            catch (Exception ex)
            {
                var idError = await RegistrarAsync(context, ex);
                await ResponderAsync(context, idError);
            }
        }

        /// <summary>Guarda el error en la base. Nunca lanza: si la base falla, deja el rastro en el log del servidor.</summary>
        private async Task<int> RegistrarAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en {Ruta}", context.Request.Path);

            try
            {
                // El middleware es singleton y el repositorio es scoped: se abre un ámbito propio.
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ILogErrorRepository>();

                var ruta = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
                return await repo.RegistrarAsync(ex.Message, ex.ToString(), ruta, ObtenerUsuario(context));
            }
            catch (Exception exLog)
            {
                _logger.LogError(exLog, "No se pudo guardar el error en dbo.LogErrores.");
                return 0;
            }
        }

        /// <summary>Correo del usuario que provocó el error: primero la sesión, si no el claim del JWT.</summary>
        private static string? ObtenerUsuario(HttpContext context)
        {
            string? correo = null;

            // La sesión solo está disponible si UseSession ya corrió para esta petición.
            try
            {
                correo = context.Session.GetString("Correo");
            }
            catch (InvalidOperationException)
            {
                // La sesión no está configurada en esta parte de la tubería: se ignora.
            }

            return correo
                ?? context.User?.FindFirst(ClaimTypes.Email)?.Value
                ?? context.User?.Identity?.Name;
        }

        /// <summary>Las rutas /api devuelven JSON; el resto va a la pantalla de error del sitio.</summary>
        private static async Task ResponderAsync(HttpContext context, int idError)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    mensaje = "Ocurrió un error inesperado. El detalle quedó registrado en el sistema.",
                    idError
                }));
                return;
            }

            context.Response.Redirect($"/Home/Error?idError={idError}");
        }
    }
}
