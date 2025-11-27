using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class ResetPasswordResponseDTO
	{
		public bool Success { get; set; }
		public string NuevaPassword { get; set; }
		public string Mensaje { get; set; }
		public int UsuarioId { get; set; }
		public string Email { get; set; }
		public string NombreCompleto { get; set; }
	}
}
