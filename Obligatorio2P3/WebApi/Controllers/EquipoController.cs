using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Microsoft.Extensions.Logging;

namespace Obligatorio.WebApp2.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Gerente")]
	public class EquiposController : ControllerBase
	{
		private readonly IServicioPagoCU _servicioPago;
		private readonly ILogger<EquiposController> _logger;

		public EquiposController(IServicioPagoCU servicioPago, ILogger<EquiposController> logger)
		{
			_servicioPago = servicioPago;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene los equipos cuyos empleados realizaron pagos individuales mayores al monto indicado.
		/// </summary>
		/// <param name="monto">Monto mínimo (decimal)</param>
		/// <param name="includeUsers">Incluir lista de usuarios por equipo (opcional)</param>
		[HttpGet("pagos-mayores/{monto}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetEquiposConPagosMayores([FromRoute] decimal monto, [FromQuery] bool includeUsers = false)
		{
			try
			{
				if (monto < 0) return BadRequest(new { Message = "El monto debe ser mayor o igual a 0." });

				var result = await _servicioPago.ObtenerEquiposConPagosMayoresAsync(monto, includeUsers);

				if (result == null || !result.Any()) return NotFound(new { Message = "No se encontraron equipos con pagos mayores al monto indicado." });

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener equipos con pagos mayores a {Monto}", monto);
				return StatusCode(500, "Error interno al obtener equipos.");
			}
		}
	}
}
