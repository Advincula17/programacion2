using API_Cursos_Online.Interfaces;
using API_Cursos_Online.Models;
using API_Cursos_Online.DTOs;
using API_Cursos_Online.Context;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos_Online.Services
{
    public class CursoService : ICursoService
    {
        private readonly BibliotecaContext _context;

        public CursoService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task CrearCurso(CrearCursoDTO dto)
        {
            var curso = new Curso
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                ProfesorId = dto.ProfesorId
            };
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCurso(int id, ActualizarCursoDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) throw new Exception("Curso no encontrado");

            curso.Titulo = dto.Titulo;
            curso.Descripcion = dto.Descripcion;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) throw new Exception("Curso no encontrado");

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }

        public async Task InscribirEstudiante(int cursoId, int estudianteId)
        {
            var curso = await _context.Cursos
                .Include(c => c.EstudiantesCursos)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null) throw new Exception("Curso no encontrado");

            if (curso.EstudiantesCursos.Count >= 100)
                throw new Exception("El curso ya tiene el límite máximo de estudiantes inscritos");

            var estudianteCurso = new EstudianteCurso
            {
                CursoId = cursoId,
                EstudianteId = estudianteId
            };

            _context.EstudiantesCursos.Add(estudianteCurso);
            await _context.SaveChangesAsync();
        }
    }
}
