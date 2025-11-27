using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{
	public class TipoGasto
	{

		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public bool Activo { get; set; }


    }
}
