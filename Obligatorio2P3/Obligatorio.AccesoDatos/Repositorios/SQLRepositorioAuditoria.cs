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

		public SQLRepositorioAuditoria(OblContext context)
		{
			_context = context;
		}

		public void Agregar(Auditoria auditoria)
		{
			_context.Set<Auditoria>().Add(auditoria);
			_context.SaveChanges();
		}

		public IEnumerable<Auditoria> ObtenerPorIdTipoGasto(int idTipoGasto)
		{
			// Incluimos Usuario para saber quién fue
			return _context.Set<Auditoria>()
						   .Include(a => a.Usuario)
						   .Where(a => a.TipoGastoId == idTipoGasto)
						   .OrderByDescending(a => a.FechaOperacion)
						   .ToList();
		}
	}

}
