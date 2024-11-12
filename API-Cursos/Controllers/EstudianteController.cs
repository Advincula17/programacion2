using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API_Cursos_Online.Interfaces;
using API_Cursos_Online.DTOs;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class EstudianteController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;

    public EstudianteController(IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    [HttpPost("registrarse")]
    public async Task<IActionResult> RegistrarEstudiante(RegistrarEstudianteDTO dto)
    {
        await _estudianteService.RegistrarEstudiante(dto);
        return Ok("Estudiante registrado exitosamente");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _estudianteService.Login(dto);
        return Ok(new { Token = token });
    }
}
