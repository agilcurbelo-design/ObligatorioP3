
using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs
{
	public class UsuarioDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string Email { get; set; }

		// Guardar como string para facilitar serialización
		public string Rol { get; set; }

	}
}