using Microsoft.AspNetCore.Identity;
using Obligatorio.AccesoDatos.EntityFramework;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.Excepciones;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.AccesoDatos.EntityFramework.RepositoriosEF
{
    public class RepositorioUsuarioEF : IRepositorioUsuario
    {
        public OblContext _context;
		private readonly PasswordHasher<Usuario> _passwordHasher;

		public RepositorioUsuarioEF(OblContext context)
        {
            _context = context;
			_passwordHasher = new PasswordHasher<Usuario>();
		}

        public void Add(Usuario nuevo)
        {

            _context.Usuarios.Add(nuevo);
            _context.SaveChanges();
        }

        public IEnumerable<Usuario> FindAll()
        {
            return _context.Usuarios.
                OrderBy(user => user.Nombre).
                ThenByDescending(user => user.Id);
        }

        public Usuario FindById(int id)
        {
            throw new NotImplementedException();
        }

		public Usuario Login(string email, string contrasenia)
		{
			List<Usuario> todos = _context.Usuarios.ToList();

		

			Usuario usuario = todos.FirstOrDefault(u =>
				u.Email.ToLower() == email.ToLower() &&
				u.Contrasenia == contrasenia);

			if (usuario == null)
				throw new UsuarioException("Usuario o contraseña incorrecta.");

			return usuario;
		}



		public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario nuevo)
        {
            throw new NotImplementedException();
        }
		// Implementación de ActualizarPassword(int usuarioId, string nuevaPassword)
		public void ActualizarPassword(int usuarioId, string nuevaPassword)
		{
			if (string.IsNullOrWhiteSpace(nuevaPassword))
				throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(nuevaPassword));

			var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
			if (usuario == null)
				throw new InvalidOperationException($"Usuario con id {usuarioId} no encontrado.");

			// Hashear la contraseña y guardarla (suponiendo que la propiedad se llama Contrasenia)
			usuario.Contrasenia = _passwordHasher.HashPassword(usuario, nuevaPassword);

			_context.SaveChanges();
		}
		public Usuario ObtenerPorId(int id)
		{
			// Si preferís que devuelva null en vez de lanzar, cambiá FirstOrDefault por SingleOrDefault y no lances.
			return _context.Usuarios.FirstOrDefault(u => u.Id == id);
		}

		public bool ExisteEmail(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }

		public Task<Usuario> ObtenerPorIdAsync(int usuario)
		{
			throw new NotImplementedException();
		}
	}
}
