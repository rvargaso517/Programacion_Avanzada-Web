namespace Tarea1.Models
{
    /// <summary>
    /// Resultado estándar de una operación de servicio. Evita usar excepciones
    /// para el flujo de control (credenciales inválidas, validaciones, etc.).
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public T? Data { get; init; }

        public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static ServiceResult<T> Fail(string error) => new() { Success = false, Error = error };
    }
}
