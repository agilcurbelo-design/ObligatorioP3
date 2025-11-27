
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.InterfacesRepositorio
{
	public interface IRepositorioUsuario : IRepositorio<Usuario>
	{
        void Add(Usuario usuario);
        bool ExisteEmail(string email);
        public Usuario Login(string email, string pass);
		Usuario ObtenerPorId(int id);
		void ActualizarPassword(int usuarioId, string nuevaPassword);
		Task<Usuario> ObtenerPorIdAsync(int usuario);



	}


}
