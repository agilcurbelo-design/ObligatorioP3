using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.InterfacesRepositorio
{
	public interface IRepositorioAuditoria
	{
		Task<IEnumerable<Auditoria>> ObtenerPorTipoGastoAsync(
			int tipoGastoId, DateTime? desde = null, DateTime? hasta = null, string? accion = null);
		Task AgregarAsync(Auditoria audit);
	}


}
