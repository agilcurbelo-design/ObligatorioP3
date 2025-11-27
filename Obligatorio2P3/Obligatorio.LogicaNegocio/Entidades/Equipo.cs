using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{
	public class Equipo
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public virtual List<Usuario> Usuarios { get; set; } = new();
	}
}
