using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API_Cursos_Online.Interfaces;
using API_Cursos_Online.DTOs;

[Authorize(Roles = "Profesor")]
[Route("api/[controller]")]
[ApiController]
public class CursoController : ControllerBase
{
    private readonly ICursoService _cursoService;

    public CursoController(ICursoService cursoService)
    {
        _cursoService = cursoService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearCurso(CrearCursoDTO dto)
    {
        await _cursoService.CrearCurso(dto);
        return Ok("Curso creado exitosamente");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarCurso(int id, ActualizarCursoDTO dto)
    {
        await _cursoService.ActualizarCurso(id, dto);
        return Ok("Curso actualizado exitosamente");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCurso(int id)
    {
        await _cursoService.EliminarCurso(id);
        return Ok("Curso eliminado exitosamente");
    }

    [Authorize(Roles = "Estudiante")]
    [HttpPost("{id}/inscribir")]
    public async Task<IActionResult> InscribirEstudiante(int id, InscripcionDTO dto)
    {
        await _cursoService.InscribirEstudiante(id, dto.EstudianteId);
        return Ok("Estudiante inscrito exitosamente");
    }
}
