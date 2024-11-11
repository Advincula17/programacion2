using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_biblioteca.Models;

namespace api_biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCursos()
        {
            return Ok(await _context.Cursos.Include(c => c.Profesor).Include(c => c.Estudiantes).ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CrearCurso(Curso curso)
        {
            if (curso.Estudiantes.Count > 100)
            {
                return BadRequest("No se puede inscribir más de 100 estudiantes en un curso.");
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCursos), new { id = curso.Id }, curso);
        }

        // Otros métodos (Actualizar, Eliminar, etc.)...
    }
}
