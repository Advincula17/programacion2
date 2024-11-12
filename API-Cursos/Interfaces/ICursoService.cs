using API_Cursos_Online.DTOs;
namespace API_Cursos_Online.Interfaces
{
    public interface ICursoService
    {
        Task CrearCurso(CrearCursoDTO dto);
        Task ActualizarCurso(int id, ActualizarCursoDTO dto);
        Task EliminarCurso(int id);
        Task InscribirEstudiante(int cursoId, int estudianteId);
    }
}
