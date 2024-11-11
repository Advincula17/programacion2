using Microsoft.EntityFrameworkCore;

namespace api_biblioteca.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Estudiantes)
                .WithMany(e => e.Cursos)
                .UsingEntity(j => j.ToTable("CursoEstudiante"));
        }
    }
}
