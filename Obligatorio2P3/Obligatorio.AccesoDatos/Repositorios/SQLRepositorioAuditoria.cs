using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
namespace Obligatorio.AccesoDatos.Repositorios
{
	

	public class SQLRepositorioAuditoria : IRepositorioAuditoria
	{
		private readonly OblContext _context;
		public SQLRepositorioAuditoria(OblContext context) => _context = context;

		public async Task AgregarAsync(Auditoria audit)
		{
			await _context.Auditorias.AddAsync(audit);
		}

		public async Task<IEnumerable<Auditoria>> ObtenerPorTipoGastoAsync(
			int tipoGastoId, DateTime? desde = null, DateTime? hasta = null, string? accion = null)
		{
			var q = _context.Auditorias
				.AsNoTracking()
				.Include(a => a.Usuario)
				.Where(a => a.TipoGastoId == tipoGastoId);

			if (desde.HasValue) q = q.Where(a => a.Fecha >= desde.Value);
			if (hasta.HasValue) q = q.Where(a => a.Fecha <= hasta.Value);
			if (!string.IsNullOrWhiteSpace(accion)) q = q.Where(a => a.Accion == accion);

			return await q.OrderByDescending(a => a.Fecha).ToListAsync();
		}

	}

}
