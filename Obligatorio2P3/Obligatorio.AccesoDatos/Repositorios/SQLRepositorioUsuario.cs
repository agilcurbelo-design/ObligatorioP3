using Obligatorio.AccesoDatos.EntityFramework;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;


namespace Obligatorio.AccesoDatos.Repositorios
{
	public class SQLRepositorioUsuario : IRepositorioUsuario
	{
		private readonly OblContext _context;

		public SQLRepositorioUsuario(OblContext context)
		{
			_context = context;
		}

		public Usuario Login(string email, string password)
		{
			return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Contrasenia == password);
		}

		public void Add(Usuario item)
		{
			try
			{

				_context.Usuarios.Add(item);
				_context.SaveChanges();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void Delete(Usuario item)
		{
			throw new NotImplementedException();
		}

		public bool ExisteEmail(string email)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Usuario> FindAll()
		{
			throw new NotImplementedException();
		}

		public Usuario FindById(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Usuario> GetAll()
		{
			throw new NotImplementedException();
		}

		public Usuario GetById(int id)
		{
			throw new NotImplementedException();
		}


		public void Remove(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(Usuario item)
		{
			throw new NotImplementedException();
		}

		public Usuario ObtenerPorId(int id)
		{
			return _context.Usuarios.Find(id);
		}

		// RF3 - Actualizar contraseña
		public void ActualizarPassword(int usuarioId, string nuevaPassword)
		{
			var usuario = ObtenerPorId(usuarioId);
			if (usuario != null)
			{
				// Actualizar la propiedad privada mediante reflexión o hacerlo directamente
				usuario.Contrasenia = nuevaPassword;
				_context.SaveChanges();
			}
		}

		public async Task<Usuario> ObtenerPorIdAsync(int usuario)
		{
			return await _context.Usuarios
	   .FirstOrDefaultAsync(u => u.Id == usuario);
		}
	}
}
