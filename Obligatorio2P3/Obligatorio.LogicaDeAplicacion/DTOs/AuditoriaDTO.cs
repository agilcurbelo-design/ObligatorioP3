using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class AuditoriaDTO
	{
		public int Id;
		public DateTime Fecha { get; set; }
		public string Operacion { get; set; }
		public string NombreUsuario { get; set; }
		public string Detalle { get; set; }
	}
}
