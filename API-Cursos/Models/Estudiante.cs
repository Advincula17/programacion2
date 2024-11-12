namespace API_Cursos_Online.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Para almacenar la contraseña de forma segura

        // Relación muchos a muchos con Curso
        public ICollection<EstudianteCurso> EstudiantesCursos { get; set; }
    }
}
