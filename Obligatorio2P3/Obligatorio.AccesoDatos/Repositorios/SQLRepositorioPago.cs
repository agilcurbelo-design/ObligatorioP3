using Microsoft.EntityFrameworkCore;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
namespace Obligatorio.AccesoDatos.Repositorios
{
	public class SQLRepositorioPago : IRepositorioPago
	{
		private readonly OblContext _context;
		public SQLRepositorioPago(OblContext context) => _context = context;


		public async Task<List<Equipo>> ObtenerEquiposConPagoUnicoMayorAsync(decimal monto)
		{
			return await _context.Pagos
				.Where(p => p.Monto > monto && p.Usuario.EquipoId != null)
				.Select(p => p.Usuario.Equipo!)
				.Distinct()
				.OrderByDescending(e => e.Nombre)
				.ToListAsync();
		}


		public async Task AgregarAsync(Pago pago)
		{
			// Si Pago es abstracto y Unico/Recurrente son subclases, EF las manejará
			await _context.Pagos.AddAsync(pago);
		}

		public async Task UnitOfWorkSaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task<List<Pago>> ObtenerPorUsuarioAsync(int usuarioId)
		{
			return await _context.Pagos
				.Where(p => p.UsuarioId == usuarioId)
				.Include(p => p.TipoGasto)
				.ToListAsync();
		}

		public void Add(Pago obj)
		{
			throw new NotImplementedException();
		}

		public Pago Create(Pago pago)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Pago> FindAll()
		{
			throw new NotImplementedException();
		}

		public Pago FindById(int id)
		{
			throw new NotImplementedException();
		}

		public Pago GetById(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Pago> GetPagoPorMes(int mes, int anio)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Pago>> GetPagosPorUsuarioAsync(int usuarioId)
		{
			return await _context.Pagos
				.Include(p => p.TipoGasto)
				.Where(p => p.UsuarioId == usuarioId)
				.OrderByDescending(p => p.Fecha)
				.ToListAsync();
		}

		public void Remove(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(Pago obj)
		{
			throw new NotImplementedException();
		}

		public async Task<Pago> ObtenerPorIdAsync(int id)
		{
			// Carga el pago y sus relaciones para evitar problemas de navegación nula
			var pago = await _context.Pagos
				.Include(p => p.Usuario)
				.Include(p => p.TipoGasto)
				.FirstOrDefaultAsync(p => p.Id == id);

			return pago; // puede ser null si no existe (ajusta la firma si querés reflejar nullabilidad)
		}

	}
}
