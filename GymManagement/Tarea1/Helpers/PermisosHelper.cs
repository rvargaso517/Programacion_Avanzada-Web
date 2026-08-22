using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Tarea1.Helpers
{
    public static class PermisosHelper
    {
        private static readonly string PathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "permisos.json");

        public static Dictionary<string, List<string>> CargarPermisos()
        {
            if (!File.Exists(PathFile))
            {
                var defaultPermisos = new Dictionary<string, List<string>>
                {
                    { "1", new List<string> { "Dashboard", "Clientes", "Oportunidades", "Planes", "Membresias", "Agenda", "Rutinas", "Reservas", "Pagos", "Usuarios", "Contenido Web", "Perfil" } },
                    { "2", new List<string> { "Dashboard", "Clientes", "Oportunidades", "Agenda", "Reservas", "Pagos", "Perfil" } },
                    { "3", new List<string> { "Dashboard", "Agenda", "Rutinas", "Reservas", "Perfil" } },
                    { "4", new List<string> { "Agenda", "Reservas", "Perfil" } }
                };
                GuardarPermisos(defaultPermisos);
                return defaultPermisos;
            }

            try
            {
                var json = File.ReadAllText(PathFile);
                return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new();
            }
            catch
            {
                return new();
            }
        }

        public static void GuardarPermisos(Dictionary<string, List<string>> permisos)
        {
            var dir = Path.GetDirectoryName(PathFile);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonSerializer.Serialize(permisos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(PathFile, json);
        }

        public static bool TieneAcceso(int rolId, string menu)
        {
            var permisos = CargarPermisos();
            var rolKey = rolId.ToString();
            if (permisos.ContainsKey(rolKey))
            {
                return permisos[rolKey].Contains(menu);
            }
            return false;
        }

        public static List<string> ListarTodosLosMenus()
        {
            return new List<string>
            {
                "Dashboard",
                "Clientes",
                "Oportunidades",
                "Planes",
                "Membresias",
                "Agenda",
                "Rutinas",
                "Reservas",
                "Pagos",
                "Usuarios",
                "Contenido Web"
            };
        }
    }
}
