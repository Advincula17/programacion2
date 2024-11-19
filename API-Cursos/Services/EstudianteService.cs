using API_Cursos_Online.Interfaces;
using API_Cursos_Online.Models;
using API_Cursos_Online.DTOs;
using API_Cursos_Online.Context;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos_Online.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly BibliotecaContext _context;
        private readonly IConfiguration _configuration;

        public EstudianteService(BibliotecaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task RegistrarEstudiante(RegistrarEstudianteDTO dto)
        {
            var estudiante = new Estudiante
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // Hashear la contraseña
            };
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            // Mensaje de depuración en consola
            Console.WriteLine($"Estudiante registrado correctamente: {estudiante.Nombre} ({estudiante.Email})");
        }

        public async Task<string> Login(LoginDTO dto)
        {
            // Buscar al estudiante por correo electrónico
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            if (estudiante == null || !BCrypt.Net.BCrypt.Verify(dto.Password, estudiante.PasswordHash))
                throw new Exception("Credenciales inválidas");

            // Verificar configuraciones del JWT
            var keyValue = _configuration["Jwt:Key"] ?? throw new Exception("La clave JWT (Jwt:Key) no está configurada.");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("El emisor (Jwt:Issuer) no está configurado.");
            var audience = _configuration["Jwt:Audience"] ?? throw new Exception("La audiencia (Jwt:Audience) no está configurada.");

            // Generar token JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, estudiante.Id.ToString()),
                new Claim(ClaimTypes.Email, estudiante.Email),
                new Claim(ClaimTypes.Role, "Estudiante")
            };

            // Generar clave y credenciales de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            // Mensaje de depuración en consola
            Console.WriteLine("Token generado correctamente para el estudiante:");
            Console.WriteLine($"Nombre: {estudiante.Nombre}");
            Console.WriteLine($"Email: {estudiante.Email}");
            Console.WriteLine($"ID: {estudiante.Id}");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
