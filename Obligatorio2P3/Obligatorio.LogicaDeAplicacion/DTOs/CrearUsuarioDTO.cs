using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obligatorio.LogicaNegocio.Entidades;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
    public class CrearUsuarioDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        // El email se genera automáticamente en la capa de aplicación
        // por lo tanto no se incluye en este DTO de entrada.

        public string Password { get; set; } = string.Empty;

        // Solo puede ser "Gerente" o "Empleado"
        public Rol Rol { get; set; }
        public int EquipoId { get; set; }
    }
}
