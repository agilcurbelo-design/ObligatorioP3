using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	internal class EquipoConUsuarioDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;

		public List<UsuarioDTO> Usuarios { get; set; } = new List<UsuarioDTO>();
	}
}
