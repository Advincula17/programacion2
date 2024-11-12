public class Profesor
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public List<Curso> Cursos { get; set; } = new List<Curso>();
}
