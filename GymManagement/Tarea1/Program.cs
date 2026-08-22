using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tarea1.Security;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Registros de Inyección de Dependencias (DI)
builder.Services.AddSingleton<Tarea1.Data.IDbConnectionFactory, Tarea1.Data.SqlConnectionFactory>();
builder.Services.AddScoped<Tarea1.Repositories.IUsuarioRepository, Tarea1.Repositories.UsuarioRepository>();
builder.Services.AddScoped<Tarea1.Repositories.IRolRepository, Tarea1.Repositories.RolRepository>();
builder.Services.AddScoped<Tarea1.Repositories.IRecuperacionRepository, Tarea1.Repositories.RecuperacionRepository>();
builder.Services.AddScoped<Tarea1.Repositories.IRutinaRepository, Tarea1.Repositories.RutinaRepository>();
builder.Services.AddScoped<Tarea1.Repositories.IReservaRepository, Tarea1.Repositories.ReservaRepository>();
builder.Services.AddScoped<Tarea1.Repositories.ILogErrorRepository, Tarea1.Repositories.LogErrorRepository>();

builder.Services.AddScoped<Tarea1.Security.IPasswordHasher, Tarea1.Security.BCryptPasswordHasher>();
builder.Services.AddScoped<Tarea1.Security.IJwtTokenGenerator, Tarea1.Security.JwtTokenGenerator>();

builder.Services.AddScoped<Tarea1.Services.IUsuarioService, Tarea1.Services.UsuarioService>();
builder.Services.AddScoped<Tarea1.Services.IAuthService, Tarea1.Services.AuthService>();
builder.Services.AddScoped<Tarea1.Services.EmailService>();

// ---------------------------------------------------------------------------
// JWT: configuración y validación del token
// ---------------------------------------------------------------------------
// Los valores se leen de la sección "Jwt" de appsettings.json y deben ser los
// mismos que usa GymManagement_API para que los tokens sirvan en ambos lados.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "Falta la sección 'Jwt' en appsettings.json. Sin ella no se pueden emitir ni validar tokens.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };

        // El navegador no manda el encabezado "Authorization", así que también se
        // acepta el token desde la cookie HttpOnly que se crea al iniciar sesión.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (string.IsNullOrEmpty(context.Token))
                {
                    var cookie = context.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(cookie))
                        context.Token = cookie;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// HttpClient para consumir la API
builder.Services.AddHttpClient();

// Sesiones
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware propio: captura cualquier excepción no controlada y la guarda en
// la tabla dbo.LogErrores. Va de primero para que envuelva a todo lo demás.
app.UseMiddleware<Tarea1.Middleware.ManejoExcepcionesMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
