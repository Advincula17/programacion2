namespace api_biblioteca.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}
