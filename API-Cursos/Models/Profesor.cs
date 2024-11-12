namespace API_Cursos_Online.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Para almacenar la contraseña de forma segura

        // Relación uno a muchos con Curso
        public ICollection<Curso> Cursos { get; set; }
    }
}
