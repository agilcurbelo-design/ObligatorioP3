using System;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.Mappers;
using Obligatorio.LogicaNegocio.Entidades;  
using Obligatorio.LogicaNegocio.InterfacesRepositorio;

namespace Obligatorio.LogicaDeAplicacion.CasosDeUso.Usuarios
{
    public class CrearUsuarioCU
    {
        private readonly IRepositorioUsuario _repo;

        // Constructor clásico
        public CrearUsuarioCU(IRepositorioUsuario repo)
        {
            _repo = repo;
        }

        public void Ejecutar(CrearUsuarioDTO dto)
        {
            // Generar email automático
            string baseEmail = (dto.Nombre.Substring(0, Math.Min(3, dto.Nombre.Length)) +
                                dto.Apellido.Substring(0, Math.Min(3, dto.Apellido.Length)))
                .ToLower();

            string email = $"{baseEmail}@miapp.com";
            int contador = 1;

            while (_repo.ExisteEmail(email))
            {
                email = $"{baseEmail}{contador}@miapp.com";
                contador++;
            }

            // Mapear DTO → Entidad
            Usuario usuario = UsuarioMapper.ToEntity(dto, email);

            // Guardar
            _repo.Add(usuario);
        }
    }
}