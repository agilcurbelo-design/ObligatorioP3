

namespace WebApp.DTOs
{
	// LoginResponseDTO.cs
	public class LoginResponseDTO
	{
		public string Token { get; set; } = string.Empty;

		// ¡CAMBIO CLAVE! Esto mapea el objeto "usuario" del JSON de la API
		public UsuarioDTO Usuario { get; set; }
	}
}
