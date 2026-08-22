namespace Tarea1.Security
{
    /// <summary>Configuración del JWT leída desde appsettings ("Jwt").</summary>
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiraMinutos { get; set; } = 120;
    }
}
