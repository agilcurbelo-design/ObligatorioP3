using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaNegocio.Entidades;
namespace Obligatorio.LogicaNegocio.InterfacesRepositorio
{
	public interface IRepositorioTipoGasto
	{
		Task<TipoGasto> ObtenerPorIdAsync(int tipoGasto);
	}
}
