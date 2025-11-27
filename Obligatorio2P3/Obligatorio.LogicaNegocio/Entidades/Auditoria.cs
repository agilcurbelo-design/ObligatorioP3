using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{

	public class Auditoria
	{
		public int Id { get; set; }

		public int TipoGastoId { get; set; }
		public string Accion { get; set; } // "Alta", "Edición", "Baja"
		public DateTime Fecha { get; set; }

		public int UsuarioId { get; set; }
		public Usuario Usuario { get; set; }
	}

}
