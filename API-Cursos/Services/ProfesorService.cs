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
    public class ProfesorService : IProfesorService
    {
        private readonly BibliotecaContext _context;
        private readonly IConfiguration _configuration;

        public ProfesorService(BibliotecaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task RegistrarProfesor(RegistrarProfesorDTO dto)
        {
            var profesor = new Profesor
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // Hashear la contraseña
            };
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();
        }

        public async Task<string> Login(LoginDTO dto)
        {
            // Buscar al profesor por correo electrónico
            var profesor = await _context.Profesores
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (profesor == null || !BCrypt.Net.BCrypt.Verify(dto.Password, profesor.PasswordHash))
                throw new Exception("Credenciales inválidas");

            // Validar configuraciones JWT
            var keyValue = _configuration["Jwt:Key"] ?? throw new Exception("La clave JWT (Jwt:Key) no está configurada.");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("El emisor (Jwt:Issuer) no está configurado.");
            var audience = _configuration["Jwt:Audience"] ?? throw new Exception("La audiencia (Jwt:Audience) no está configurada.");

            // Definir los claims del profesor
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, profesor.Id.ToString()),
                new Claim(ClaimTypes.Email, profesor.Email),
                new Claim(ClaimTypes.Role, "Profesor")
            };

            // Generar clave de seguridad y credenciales de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generar el token JWT
            try
            {
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: creds);

                // Mensajes de depuración para inspección
                Console.WriteLine("Token generado correctamente para el profesor:");
                Console.WriteLine($"Nombre: {profesor.Nombre}");
                Console.WriteLine($"Email: {profesor.Email}");
                Console.WriteLine($"ID: {profesor.Id}");
                Console.WriteLine($"Token: {new JwtSecurityTokenHandler().WriteToken(token)}");

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el token: {ex.Message}");
                throw;
            }
        }
    }
}
