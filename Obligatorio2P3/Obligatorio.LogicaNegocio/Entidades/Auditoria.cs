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
			public DateTime FechaOperacion { get; set; }
			public string TipoOperacion { get; set; } // "Alta", "Baja", "Modificacion"

			// El nombre del tipo de gasto en el momento de la operación
			// (Guardamos el nombre por si el original se borra)
			public string NombreTipoGasto { get; set; }
			public int TipoGastoId { get; set; }

			// Quién hizo la operación
			public int UsuarioId { get; set; }
			public virtual Usuario Usuario { get; set; }
		}
	}


