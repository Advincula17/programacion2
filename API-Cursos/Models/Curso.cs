namespace API_Cursos_Online.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int ProfesorId { get; set; } // Foreign key

        // Relación con Profesor
        public Profesor Profesor { get; set; }

        // Relación muchos a muchos con Estudiante
        public ICollection<EstudianteCurso> EstudiantesCursos { get; set; }
    }
}
