using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaDeAplicacion.DTOs;
namespace Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso
{
	public interface IAuditoriaCU
	{
		Task<IEnumerable<AuditoriaDTO>> ObtenerAuditoriaTipoGastoAsync(
			int tipoGastoId, DateTime? desde = null, DateTime? hasta = null, string? accion = null);
	}

}
