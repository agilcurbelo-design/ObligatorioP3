using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaNegocio.Entidades;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class PagoDTO
	{
		public int Id { get; set; }
		public decimal Monto { get; set; }
		public DateTime Fecha { get; set; }
		public int TipoGastoId { get; set; }
		public int UsuarioId { get; set; }
		public MetodoPago MetodoPago { get; set; }
		public string Descripcion { get; set; }
	}


}

