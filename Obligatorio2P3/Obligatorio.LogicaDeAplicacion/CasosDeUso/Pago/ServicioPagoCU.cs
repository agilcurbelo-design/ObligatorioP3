// ServicioPagoCU.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obligatorio.AccesoDatos;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ServicioPagoCU : IServicioPagoCU
{
	private readonly IRepositorioPago _repoPago;
	private readonly IRepositorioUsuario _repoUsuario;
	private readonly IRepositorioTipoGasto _repoTipoGasto;
	private readonly ILogger<ServicioPagoCU> _logger;


	public ServicioPagoCU(IRepositorioPago repoPago, IRepositorioUsuario repoUsuario, IRepositorioTipoGasto repoTipoGasto, ILogger<ServicioPagoCU> logger)
	{
		_repoPago = repoPago;
		_repoUsuario = repoUsuario;
		_repoTipoGasto = repoTipoGasto;
		_logger = logger;
	}


	public async Task<IEnumerable<PagoDTO>> ObtenerPagosPorUsuarioAsync(int usuarioId)
	{
		if (usuarioId <= 0)
		{
			_logger.LogWarning("ObtenerPagosPorUsuarioAsync llamado con usuarioId inválido: {UsuarioId}", usuarioId);
			return Enumerable.Empty<PagoDTO>();
		}

		try
		{
			// Llamada al repositorio (ya debe incluir TipoGasto)
			var pagos = await _repoPago.GetPagosPorUsuarioAsync(usuarioId);

			if (pagos == null || !pagos.Any())
			{
				_logger.LogInformation("No se encontraron pagos para usuario {UsuarioId}", usuarioId);
				return Enumerable.Empty<PagoDTO>();
			}

			// Mapeo a DTOs (manual, claro y controlable)
			var pagosDto = pagos.Select(p => new PagoDTO
			{
				Id = p.Id,
				Monto = p.Monto,
				Fecha = p.Fecha,
				TipoGastoId = p.TipoGastoId,
			}).ToList();

			return pagosDto;
		}
		catch (DbUpdateException dbEx)
		{
			_logger.LogError(dbEx, "Error de BD al obtener pagos del usuario {UsuarioId}", usuarioId);
			throw; // dejar que el controller maneje el 500 o envolver en excepción de aplicación si preferís
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error inesperado al obtener pagos del usuario {UsuarioId}", usuarioId);
			throw;
		}


	}
	public async Task<List<EquipoDto>> ObtenerEquiposConPagosMayoresAsync(decimal monto, bool includeUsers = false)
	{
		var equipos = await _repoPago.ObtenerEquiposConPagoUnicoMayorAsync(monto);
		if (!includeUsers)
			return equipos.Select(e => new EquipoDto { Id = e.Id, Nombre = e.Nombre }).ToList();

		return equipos.Select(e => new EquipoConUsuarioDTO
		{
			Id = e.Id,
			Nombre = e.Nombre,
			Usuarios = e.Usuarios.Select(u => new UsuarioDTO { Id = u.Id, Nombre = u.Nombre }).ToList()
		}).Cast<EquipoDto>().ToList();
	}

	public async Task<PagoDTO> CrearPagoAsync(CrearPagoDTO dto)
	{
		// Validaciones básicas
		if (dto == null) throw new ArgumentNullException(nameof(dto));
		if (dto.Monto <= 0) throw new ArgumentException("El monto debe ser mayor a 0.", nameof(dto.Monto));

		// Obtener usuario y tipo de gasto (métodos async que devuelven null si no existe)
		var usuario = await _repoUsuario.ObtenerPorIdAsync(dto.UsuarioId);
		if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado.");
		
		var tipo = await _repoTipoGasto.ObtenerPorIdAsync(dto.TipoGastoId);
		if (tipo == null) throw new KeyNotFoundException("Tipo de gasto no encontrado.");

		var metodo = dto.MetodoPago;

		// Crear entidad de dominio (Pago único)
		var pago = new Unico
		{
			Monto = dto.Monto,
			MetodoPago = metodo,
			Descripcion = dto.Descripcion ?? string.Empty,
			TipoGastoId = dto.TipoGastoId,
			UsuarioId = dto.UsuarioId,
			Fecha = dto.Fecha ?? DateTime.UtcNow
		};

		try
		{
			await _repoPago.AgregarAsync(pago);                      // agrega al contexto
			await _repoPago.UnitOfWorkSaveChangesAsync();            // persiste cambios (SaveChangesAsync)
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error al crear pago para usuario {UsuarioId}", dto.UsuarioId);
			throw; // re-lanzar para que el controlador lo maneje/loguee
		}

		// Mapear a DTO de salida (incluyo Descripcion por si la necesitas)
		return new PagoDTO
		{
			Id = pago.Id,
			Monto = pago.Monto,
			Fecha = pago.Fecha,
			UsuarioId = pago.UsuarioId,
			TipoGastoId = pago.TipoGastoId,

		};
	}

	public async Task<PagoDTO> ObtenerPagoPorIdAsync(int pagoId)
	{
		try
		{
			// Obtener la entidad Pago desde el repositorio
			var pago = await _repoPago.ObtenerPorIdAsync(pagoId);

			// Si no existe, devolvemos null (el controlador puede interpretar NotFound)
			if (pago == null) return null;

			// Mapear la entidad a PagoDTO
			var dto = new PagoDTO
			{
				Id = pago.Id,
				Monto = pago.Monto,
				Fecha = pago.Fecha,
				UsuarioId = pago.UsuarioId,
				TipoGastoId = pago.TipoGastoId,
				MetodoPago = pago.MetodoPago
			};

			// Si quieres incluir datos adicionales (nombre de usuario, tipo de gasto), 
			// puedes cargarlos aquí si el repositorio no los incluye:
			// if (pago.Usuario != null) dto.UsuarioNombre = pago.Usuario.Nombre;
			// if (pago.TipoGasto != null) dto.TipoGastoNombre = pago.TipoGasto.Nombre;

			return dto;
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "Error al obtener pago por id {PagoId}", pagoId);
			throw;
		}
	}

}
