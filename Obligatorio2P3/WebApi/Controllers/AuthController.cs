using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaNegocio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Obligatorio.WebApp2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogin _loginUC;
		private readonly IConfiguration _configuration;

		public AuthController(ILogin loginUC, IConfiguration configuration)
		{
			_loginUC = loginUC;
			_configuration = configuration;
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public IActionResult Login([FromBody] LoginDTO loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				// Validar credenciales usando tu caso de uso
				var usuario = _loginUC.Login(loginDto.Email, loginDto.Contrasenia);

				if (usuario == null)
					return Unauthorized(new { message = "Credenciales inválidas" });

				// Generar token JWT
				var token = GenerarTokenJWT(usuario);

				return Ok(new
				{
					Token = token,
					usuario = new
					{
						id = usuario.Id,
						nombre = usuario.Nombre,
						apellido = usuario.Apellido,
						email = usuario.Email,
						rol = usuario.Rol // Ya sea string o enum, se serializa correctamente
					}
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = ex.Message });
			}
		}

		private string GenerarTokenJWT(UsuarioDTO usuario)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
			var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			// Convertir Rol a string si es enum
			string rolString = usuario.Rol;

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
				new Claim(ClaimTypes.Email, usuario.Email),
				new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
				new Claim(ClaimTypes.Role, rolString) // Guardamos el rol como string
            };

			var Token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(Token);
		}
	}
}