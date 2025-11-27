using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaDeAplicacion.DTOs;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class UsuarioConPagosDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string Email { get; set; }
		public string Rol { get; set; }
		public IEnumerable<PagoDTO> Pagos { get; set; } = Enumerable.Empty<PagoDTO>();
	}

}
