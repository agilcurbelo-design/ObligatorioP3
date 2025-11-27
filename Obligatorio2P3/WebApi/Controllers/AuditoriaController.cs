using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;

namespace Obligatorio.WebApp2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Administrador")] // Solo admins según RF6
	public class AuditoriaController : ControllerBase
	{
		private readonly IAuditoriaCU _auditoriaCU;

		public AuditoriaController(IAuditoriaCU auditoriaCU)
		{
			_auditoriaCU = auditoriaCU;
		}

		[HttpGet("tipoGasto/{id}")]
		public IActionResult ObtenerPorTipoGasto(int id)
		{
			var resultado = _auditoriaCU.ObtenerAuditoria(id);
			return Ok(resultado);
		}
	}

}
