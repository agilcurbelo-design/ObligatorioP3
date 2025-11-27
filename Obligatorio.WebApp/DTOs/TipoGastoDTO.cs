using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs
{
	public class TipoGastoDTO
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
		public string Nombre { get; set; } = string.Empty;

		[StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
		public string? Descripcion { get; set; }

		public bool Activo { get; set; } = true;

	}
}
