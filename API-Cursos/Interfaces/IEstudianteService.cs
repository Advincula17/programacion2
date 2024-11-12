using API_Cursos_Online.DTOs;
namespace API_Cursos_Online.Interfaces
{
    public interface IEstudianteService
    {
        Task RegistrarEstudiante(RegistrarEstudianteDTO dto);
        Task<string> Login(LoginDTO dto); // Retorna un token JWT
    }
}
