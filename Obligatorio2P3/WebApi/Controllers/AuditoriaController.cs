using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio.WebApp2.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using Obligatorio.LogicaDeAplicacion.DTOs;
	using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
	using System;
	using System.Threading.Tasks;

	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Administrador")]
	public class AuditoriaController : ControllerBase
	{
		private readonly IAuditoriaCU _servicioAuditoria;
		private readonly ILogger<AuditoriaController> _logger;
		private const int MaxItemsLimit = 2000; // evita respuestas demasiado grandes

		public AuditoriaController(IAuditoriaCU servicioAuditoria, ILogger<AuditoriaController> logger)
		{
			_servicioAuditoria = servicioAuditoria;
			_logger = logger;
		}

		// GET api/Auditoria/tipogasto/5?desde=2025-01-01&hasta=2025-12-31&accion=Alta
		[HttpGet("tipogasto/{tipoGastoId}")]
		public async Task<IActionResult> ObtenerPorTipoGasto(
			[FromRoute] int tipoGastoId,
			[FromQuery] DateTime? desde = null,
			[FromQuery] DateTime? hasta = null,
			[FromQuery] string? accion = null,
			[FromQuery] int? maxItems = null)
		{
			try
			{
				if (desde.HasValue && hasta.HasValue && desde > hasta)
					return BadRequest(new { Message = "El parámetro 'desde' no puede ser mayor que 'hasta'." });

				if (maxItems.HasValue && maxItems <= 0)
					return BadRequest(new { Message = "maxItems debe ser mayor que 0." });

				var items = await _servicioAuditoria.ObtenerAuditoriaTipoGastoAsync(tipoGastoId, desde, hasta, accion);

				// Aplicar límite si el cliente lo pide o usar límite por defecto
				var limit = maxItems ?? MaxItemsLimit;
				if (limit > MaxItemsLimit) limit = MaxItemsLimit;

				var result = items.Take(limit);

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener auditoría para TipoGasto {TipoGastoId}", tipoGastoId);
				return StatusCode(500, "Error interno al obtener auditoría.");
			}
		}

		// GET api/Auditoria/123
		[HttpGet("{id:int}")]
		public async Task<IActionResult> ObtenerPorId([FromRoute] int id)
		{
			try
			{
				var items = await _servicioAuditoria.ObtenerAuditoriaTipoGastoAsync(0); // alternativa: crear método ObtenerPorId en servicio
				var audit = items.FirstOrDefault(a => a.Id == id);
				if (audit == null) return NotFound(new { Message = "Registro de auditoría no encontrado." });
				return Ok(audit);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener auditoría por id {Id}", id);
				return StatusCode(500, "Error interno al obtener auditoría.");
			}
		}
	}

}
