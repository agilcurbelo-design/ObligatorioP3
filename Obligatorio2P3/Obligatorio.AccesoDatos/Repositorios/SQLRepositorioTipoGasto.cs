using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Obligatorio.AccesoDatos.EntityFramework;	
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Obligatorio.AccesoDatos.Repositorios
{
	public class SQLRepositorioTipoGasto : IRepositorioTipoGasto
	{
		private readonly OblContext _context;

		// Inyectar el contexto desde DI
		public SQLRepositorioTipoGasto(OblContext context)
		{
			_context = context;
		}

		public void Add(TipoGasto item)
		{
			_context.TipoGastos.Add(item);
			_context.SaveChanges();
		}

		public void Delete(TipoGasto item)
		{
			_context.TipoGastos.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<TipoGasto> GetAll()
		{
			return _context.TipoGastos.ToList();
		}

		public TipoGasto GetById(int id)
		{
			return _context.TipoGastos.Find(id);
		}

		public async Task<TipoGasto> ObtenerPorIdAsync(int id)
		{
			var tipo = await _context.TipoGastos
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == id);

			if (tipo == null)
				throw new KeyNotFoundException($"TipoGasto con id {id} no encontrado.");

			return tipo;
		}

	}
}
