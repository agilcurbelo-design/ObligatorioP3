using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Obligatorio.LogicaDeAplicacion.DTOs;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Obligatorio.WebApi.Services;
using Obligatorio.WebApp2.Servicios;

namespace Obligatorio.WebApp2.Controllers
{
	[ApiController]
	[Route("api/usuario")]
	public class UsuarioApiController : ControllerBase
	{
		private readonly IRepositorioUsuario _usuarioRepo;
		private readonly IPasswordGeneratorService _passwordGenerator;

		public UsuarioApiController(
			IRepositorioUsuario usuarioRepo,
			IPasswordGeneratorService passwordGenerator)
		{
			_usuarioRepo = usuarioRepo;
			_passwordGenerator = passwordGenerator;
		}

		/// <summary>
		/// RF3 - Resetear contraseña de un usuario (Solo Administradores)
		/// </summary>
		[HttpPost("reset-password")]
		[Authorize(Roles = "Administrador")]
		[ProducesResponseType(typeof(ResetPasswordResponseDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<ResetPasswordResponseDTO> ResetPassword(
			[FromBody] ResetPasswordRequestDTO request)
		{
			try
			{
				// 1. Validar que el usuario existe
				var usuario = _usuarioRepo.ObtenerPorId(request.UsuarioId);
				if (usuario == null)
				{
					return NotFound(new ResetPasswordResponseDTO
					{
						Success = false,
						Mensaje = $"No se encontró el usuario con ID {request.UsuarioId}"
					});
				}

				// 2. Generar nueva contraseña aleatoria
				string nuevaPassword = _passwordGenerator.GenerarPasswordAleatoria();

				// 3. Validar que cumple las reglas
				if (!_passwordGenerator.ValidarPassword(nuevaPassword))
				{
					return StatusCode(500, new ResetPasswordResponseDTO
					{
						Success = false,
						Mensaje = "Error al generar contraseña válida"
					});
				}

				// 4. Actualizar la contraseña
				_usuarioRepo.ActualizarPassword(request.UsuarioId, nuevaPassword);

				// 5. Retornar la nueva contraseña
				return Ok(new ResetPasswordResponseDTO
				{
					Success = true,
					NuevaPassword = nuevaPassword,
					UsuarioId = usuario.Id,
					Email = usuario.Email,
					NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
					Mensaje = "Contraseña restablecida exitosamente."
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new ResetPasswordResponseDTO
				{
					Success = false,
					Mensaje = $"Error interno: {ex.Message}"
				});
			}
		}
	}
}