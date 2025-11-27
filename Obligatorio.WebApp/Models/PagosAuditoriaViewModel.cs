using WebApp.DTOs;

namespace WebApp.Models
{
	public class PagosAuditoriaViewModel
	{
		public int? TipoGastoId { get; set; }

		// Resultados
		public List<AuditoriaDto> Auditorias { get; set; } = new();

		// Para llenar el select
		public List<TipoGastoDTO> TiposDeGasto { get; set; } = new();
	}

}
