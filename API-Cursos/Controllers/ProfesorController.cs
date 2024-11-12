using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API_Cursos_Online.Interfaces;
using API_Cursos_Online.DTOs;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ProfesorController : ControllerBase
{
    private readonly IProfesorService _profesorService;

    public ProfesorController(IProfesorService profesorService)
    {
        _profesorService = profesorService;
    }

    [HttpPost("registrarse")]
    public async Task<IActionResult> RegistrarProfesor(RegistrarProfesorDTO dto)
    {
        await _profesorService.RegistrarProfesor(dto);
        return Ok("Profesor registrado exitosamente");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _profesorService.Login(dto);
        return Ok(new { Token = token });
    }
}
