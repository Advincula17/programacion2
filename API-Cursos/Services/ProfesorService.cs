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
            var profesor = await _context.Profesores
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (profesor == null || !BCrypt.Net.BCrypt.Verify(dto.Password, profesor.PasswordHash))
                throw new Exception("Credenciales inválidas");

            // Generar token JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, profesor.Id.ToString()),
                new Claim(ClaimTypes.Email, profesor.Email),
                new Claim(ClaimTypes.Role, "Profesor")
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
