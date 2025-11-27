using Obligatorio.LogicaDeAplicacion.Mappers;
using Obligatorio.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
	public class UsuarioDTO
	{
		public int Id { get; set; }

		public string Nombre { get; set; }

		public string Apellido { get; set; }
		public string Email { get; set; }

		public string Contrasenia { get; set; }

		public string Rol { get; set; }
	

		public int EquipoId { get; set; }
		public IEnumerable<PagoDTO> Pagos { get; set; }

		internal static object Select(Func<object, CrearUsuarioDTO> value)
        {
            throw new NotImplementedException();
        }
    }
}
