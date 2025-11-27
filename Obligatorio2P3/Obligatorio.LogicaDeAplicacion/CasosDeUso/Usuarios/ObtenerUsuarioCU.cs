using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaDeAplicacion.Mappers;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Obligatorio.LogicaDeAplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs.CasosDeUso.Usuarios;

public class ObtenerUsuariosCU : IObtenerUsuarios
{
    private IRepositorioUsuario _repositorio;
    public ObtenerUsuariosCU(IRepositorioUsuario repositorio)
    {
        _repositorio = repositorio;
    }

    public IEnumerable<UsuarioDTO> ObtenerUsuarios()
    {
        var usuarios = _repositorio.FindAll();

       
        return (IEnumerable<UsuarioDTO>)usuarios.Select(u => UsuarioMapper.ToDTO(u)).ToList();
    }
}
