using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Obligatorio.LogicaDeAplicacion.Mappers;

namespace Obligatorio.LogicaDeAplicacion.CasosDeUso.Usuarios
{
	public class LoginCU : ILogin
	{
		private IRepositorioUsuario _repositorio;
		public LoginCU(IRepositorioUsuario repositorio)
		{
			_repositorio = repositorio;
		}

		public UsuarioDTO Login(string email, string pass)
		{
			var usuario = _repositorio.Login(email.ToLower(System.Globalization.CultureInfo.CurrentCulture), pass);

			// 🚨 SOLUCIÓN: Si el repositorio devuelve null (login fallido),
			// el Caso de Uso debe devolver null. Esto evita el error 500.
			if (usuario == null)
			{
				return null;
			}

			// Si no es null, procedemos con el mapeo a DTO.
			return new UsuarioDTO
			{
				Id = usuario.Id,
				Nombre = usuario.Nombre,
				Apellido = usuario.Apellido,
				Email = usuario.Email,
				Rol = usuario.Rol.ToString(),
				EquipoId = (int)usuario.EquipoId
			};
		}
	}
}