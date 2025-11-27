using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.Mappers
{
	public static class PagoMapper
	{
		public static PagoDTO ToDTO(Pago p)
		{
			if (p == null) return null;
			return new PagoDTO
			{
				Id = p.Id,
				Monto = p.Monto,
				Fecha = p.Fecha,
				TipoGastoNombre = p.TipoGasto.Nombre,
			};
		}
	}

}
