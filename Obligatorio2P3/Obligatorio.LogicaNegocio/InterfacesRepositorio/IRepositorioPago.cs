using Microsoft.EntityFrameworkCore;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.InterfacesRepositorio
{

	public interface IRepositorioPago : IRepositorio<Pago>
    {
		public Pago GetById(int id);
        public Pago Create(Pago pago);
        public IEnumerable<Pago> GetPagoPorMes(int mes, int anio);
		Task<List<Equipo>> ObtenerEquiposConPagoUnicoMayorAsync(decimal monto);
		Task AgregarAsync(Pago pago);
		Task<List<Pago>> ObtenerPorUsuarioAsync(int usuarioId);
		Task UnitOfWorkSaveChangesAsync();
		Task<Pago> ObtenerPorIdAsync(int id);

		
	

	}
}
