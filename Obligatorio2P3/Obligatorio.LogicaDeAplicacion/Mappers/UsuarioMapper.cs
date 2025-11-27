using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaNegocio.Entidades;

namespace Obligatorio.LogicaDeAplicacion.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario ToEntity(CrearUsuarioDTO dto, string emailGenerado)
        {
            var usuario = new Usuario(
                dto.Nombre,
                dto.Apellido,
                emailGenerado,
                dto.Password,
                dto.Rol

            );

            if (dto.EquipoId != 0)
                usuario.EquipoId = dto.EquipoId;

            return usuario;
        }

        // Para mostrar usuarios existentes
        public static UsuarioDTO ToDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                EquipoId = (int)usuario.EquipoId,
                Pagos = usuario.Pagos?.Select(PagoMapper.ToDTO).ToList() ?? new List<PagoDTO>()

			};
        }

        // Para casos donde quieras reconstruir un CrearUsuarioDTO desde la entidad
        public static CrearUsuarioDTO ToCrearDTO(Usuario usuario)
        {
            return new CrearUsuarioDTO
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Password = usuario.Contrasenia,
                Rol = usuario.Rol,
                EquipoId = usuario.EquipoId ?? 0
            };
        }
    }
}
