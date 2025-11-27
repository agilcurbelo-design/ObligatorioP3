using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// IServicioPagoCU.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Obligatorio.LogicaDeAplicacion.DTOs;

namespace Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso
{
	public interface IServicioPagoCU
	{
		Task<IEnumerable<PagoDTO>> ObtenerPagosPorUsuarioAsync(int usuarioId);
		Task<List<EquipoDto>> ObtenerEquiposConPagosMayoresAsync(decimal monto, bool includeUsers = false);
		Task<PagoDTO> CrearPagoAsync(CrearPagoDTO dto);
		Task<PagoDTO> ObtenerPagoPorIdAsync(int pago);
	}

}
