using Obligatorio.LogicaDeAplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso
{
	internal interface IObtenerPagosPorUsuario
	{
		public IEnumerable<PagoListadoDTO> ObtenerPagosPorUsuario();
	}
}
