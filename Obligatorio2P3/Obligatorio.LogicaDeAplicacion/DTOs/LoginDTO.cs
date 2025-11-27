using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "El email es requerido")]
		[EmailAddress(ErrorMessage = "Email inválido")]
		public string Email { get; set; }

		[Required(ErrorMessage = "La contraseña es requerida")]
		public string Contrasenia { get; set; }
	}
}