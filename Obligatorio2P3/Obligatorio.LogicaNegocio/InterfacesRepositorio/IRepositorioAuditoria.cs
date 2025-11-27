using Obligatorio.LogicaNegocio.Entidades;
using System.Collections.Generic;

namespace Obligatorio.LogicaNegocio.InterfacesRepositorio
{
	public interface IRepositorioAuditoria
	{
		void Agregar(Auditoria auditoria);
		IEnumerable<Auditoria> ObtenerPorIdTipoGasto(int idTipoGasto);

	}
}