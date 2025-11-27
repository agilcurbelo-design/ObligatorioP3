using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class AuditoriaDTO
	{
		public int Id { get; set; }
		public int TipoGastoId { get; set; }
		public string Accion { get; set; } = string.Empty;
		public DateTime Fecha { get; set; }
		public int UsuarioId { get; set; }
		public string? UsuarioNombre { get; set; }

	}
}
