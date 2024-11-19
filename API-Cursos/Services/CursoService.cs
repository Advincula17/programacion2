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
            // Validación básica de datos
            if (string.IsNullOrWhiteSpace(dto.Titulo)) 
                throw new Exception("El título del curso no puede estar vacío.");
            
            var curso = new Curso
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                ProfesorId = dto.ProfesorId
            };

            // Agregar curso al contexto
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            // Depuración
            Console.WriteLine($"Curso creado exitosamente: {curso.Titulo} (ID: {curso.Id}) por el profesor con ID: {curso.ProfesorId}");
        }

        public async Task ActualizarCurso(int id, ActualizarCursoDTO dto)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) throw new Exception("Curso no encontrado");

            // Actualización de propiedades
            curso.Titulo = dto.Titulo ?? curso.Titulo;
            curso.Descripcion = dto.Descripcion ?? curso.Descripcion;

            await _context.SaveChangesAsync();

            // Depuración
            Console.WriteLine($"Curso actualizado: {curso.Titulo} (ID: {curso.Id})");
        }

        public async Task EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) throw new Exception("Curso no encontrado");

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            // Depuración
            Console.WriteLine($"Curso eliminado: {curso.Titulo} (ID: {id})");
        }

        public async Task InscribirEstudiante(int cursoId, int estudianteId)
        {
            var curso = await _context.Cursos
                .Include(c => c.EstudiantesCursos)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null) throw new Exception("Curso no encontrado");

            // Validación de duplicado
            if (curso.EstudiantesCursos.Any(ec => ec.EstudianteId == estudianteId))
                throw new Exception("El estudiante ya está inscrito en este curso.");

            // Validación de límite
            if (curso.EstudiantesCursos.Count >= 100)
                throw new Exception("El curso ya tiene el límite máximo de estudiantes inscritos");

            var estudianteCurso = new EstudianteCurso
            {
                CursoId = cursoId,
                EstudianteId = estudianteId
            };

            _context.EstudiantesCursos.Add(estudianteCurso);
            await _context.SaveChangesAsync();

            // Depuración
            Console.WriteLine($"Estudiante con ID: {estudianteId} inscrito exitosamente en el curso: {curso.Titulo} (ID: {curso.Id})");
        }
    }
}
