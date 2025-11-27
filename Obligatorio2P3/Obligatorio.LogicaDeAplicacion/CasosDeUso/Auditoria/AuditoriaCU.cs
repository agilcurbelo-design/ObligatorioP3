using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Obligatorio.LogicaDeAplicacion.CasosDeUso.Auditoria
{
	public class AuditoriaCU : IAuditoriaCU
	{
		private readonly IRepositorioAuditoria _repoAuditoria;

		public AuditoriaCU(IRepositorioAuditoria repoAuditoria)
		{
			_repoAuditoria = repoAuditoria;
		}

		public IEnumerable<AuditoriaDTO> ObtenerAuditoria(int idTipoGasto)
		{
			var lista = _repoAuditoria.ObtenerPorIdTipoGasto(idTipoGasto);

			return lista.Select(a => new AuditoriaDTO
			{
				Id = a.Id,
				Fecha = a.FechaOperacion,
				Operacion = a.TipoOperacion,
				NombreUsuario = a.Usuario != null ? $"{a.Usuario.Nombre} {a.Usuario.Apellido}" : "Desconocido",
				Detalle = $"Tipo Gasto: {a.NombreTipoGasto}"
			});
		}
	}
}
