using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Obligatorio.LogicaDeAplicacion.DTOs;

namespace Obligatorio.WebApp2.Security
{
	public class ManejadorJWT
	{
		private static List<Obligatorio.LogicaDeAplicacion.DTOs.UsuarioDTO> _usuariosTest;
	



		public static string GenerarToken(Obligatorio.LogicaDeAplicacion.DTOs.UsuarioDTO usuarioDto)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			//clave secreta, generalmente se incluye en el archivo de configuración
			//Debe ser un vector de bytes 

			var clave = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE=");

			//Se incluye un claim para el rol

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, usuarioDto.Nombre),
					new Claim(ClaimTypes.Role, usuarioDto.Rol)
				}),
				Expires = DateTime.UtcNow.AddMonths(1),

				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(clave),
				SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public static Obligatorio.LogicaDeAplicacion.DTOs.UsuarioDTO ObtenerUsuario(string email)
		{
			{
				var usuario = _usuariosTest.SingleOrDefault(usr => usr.Email.ToUpper().Trim() == email.ToUpper().Trim());
				return usuario;

			}
		}
	}
}
