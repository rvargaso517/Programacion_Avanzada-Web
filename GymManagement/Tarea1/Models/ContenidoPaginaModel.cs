using System.Collections.Generic;

namespace Tarea1.Models
{
    public class ContenidoPaginaModel
    {
        public ChoseUsSection ChoseUs { get; set; } = new();
        public ClassesSection Classes { get; set; } = new();
        public TeamSection Team { get; set; } = new();
    }

    public class ChoseUsSection
    {
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public List<ChoseUsItem> Items { get; set; } = new();
    }

    public class ChoseUsItem
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class ClassesSection
    {
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public List<ClassesItem> Items { get; set; } = new();
    }

    public class ClassesItem
    {
        public string Categoria { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
    }

    public class TeamSection
    {
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public List<TeamItem> Items { get; set; } = new();
    }

    public class TeamItem
    {
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
    }
}
