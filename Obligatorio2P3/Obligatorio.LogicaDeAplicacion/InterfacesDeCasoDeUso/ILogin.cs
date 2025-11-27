using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.Mappers;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso
{
	public interface ILogin
	{
		public UsuarioDTO Login(string email, string pass);
	}
}
