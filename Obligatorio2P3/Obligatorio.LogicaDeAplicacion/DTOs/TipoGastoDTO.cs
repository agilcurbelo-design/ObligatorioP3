using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
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
