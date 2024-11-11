namespace api_biblioteca.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    }
}
