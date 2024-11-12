
namespace API_Cursos_Online.Models
{
    public class EstudianteCurso
    {
        public int CursoId { get; set; } // Foreign key hacia Curso
        public Curso Curso { get; set; }

        public int EstudianteId { get; set; } // Foreign key hacia Estudiante
        public Estudiante Estudiante { get; set; }
    }
}
