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
        }

        public async Task<string> Login(LoginDTO dto)
        {
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            if (estudiante == null || !BCrypt.Net.BCrypt.Verify(dto.Password, estudiante.PasswordHash))
                throw new Exception("Credenciales inválidas");

            // Generar token JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, estudiante.Id.ToString()),
                new Claim(ClaimTypes.Email, estudiante.Email),
                new Claim(ClaimTypes.Role, "Estudiante")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
