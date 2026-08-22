namespace Tarea1.Security
{
    /// <summary>Implementación de hashing usando BCrypt (con salt, workFactor 11).</summary>
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 11;

        public string Hash(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

        public bool Verify(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                // Hash con formato inválido -> credencial no válida.
                return false;
            }
        }
    }
}
