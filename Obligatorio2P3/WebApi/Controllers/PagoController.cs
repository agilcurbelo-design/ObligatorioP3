using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagosController : ControllerBase
{
	private readonly IServicioPagoCU _servicioPago;
	private readonly ILogger<PagosController> _logger;

	public PagosController(IServicioPagoCU servicioPago, ILogger<PagosController> logger)
	{
		_servicioPago = servicioPago;
		_logger = logger;
	}

	[HttpGet("usuario/{usuarioId}")]
	public async Task<IActionResult> GetPagosPorUsuario([FromRoute] int usuarioId)
	{
		if (!User.Identity?.IsAuthenticated ?? true)
			return Unauthorized();

		var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (!int.TryParse(claimId, out var tokenUserId))
			return Unauthorized();

		if (tokenUserId != usuarioId && !(User.IsInRole("Gerente") || User.IsInRole("Administrador")))
			return Forbid();

		try
		{
			var pagos = await _servicioPago.ObtenerPagosPorUsuarioAsync(usuarioId);
			if (pagos == null || !pagos.Any()) return NotFound(new { Message = "No se encontraron pagos para el usuario." });
			return Ok(pagos);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error interno al obtener pagos para usuario {UsuarioId}", usuarioId);
			return StatusCode(500, "Error interno al obtener pagos.");
		}
	}

	[HttpPost]

	public async Task<IActionResult> CrearPago([FromBody] CrearPagoDTO dto)
	{
		if (dto == null) return BadRequest(new { Message = "Cuerpo de la petición vacío." });
		if (!ModelState.IsValid) return BadRequest(ModelState);

		try
		{
			var pagoCreado = await _servicioPago.CrearPagoAsync(dto);
			return CreatedAtAction(nameof(ObtenerPagoPorId), new { id = pagoCreado.Id }, pagoCreado);
		}
		catch (ArgumentException aex)
		{
			return BadRequest(new { Message = aex.Message });
		}
		catch (KeyNotFoundException knf)
		{
			return BadRequest(new { Message = knf.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error al crear pago para usuario {UsuarioId}", dto.UsuarioId);
			return StatusCode(500, "Error interno al crear pago.");
		}
	}

	[HttpGet("{id}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ObtenerPagoPorId([FromRoute] int id)
	{
		try
		{
			var pago = await _servicioPago.ObtenerPagoPorIdAsync(id);
			if (pago == null) return NotFound(new { Message = "Pago no encontrado." });
			return Ok(pago);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error al obtener pago por id {PagoId}", id);
			return StatusCode(500, "Error interno al obtener pago.");
		}
	}
}

