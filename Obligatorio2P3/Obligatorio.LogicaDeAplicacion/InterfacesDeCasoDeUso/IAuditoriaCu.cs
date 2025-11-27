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
		IEnumerable<AuditoriaDTO> ObtenerAuditoria(int idTipoGasto);
	}
}


