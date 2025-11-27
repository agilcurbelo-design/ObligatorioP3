using WebApp.DTOs;

namespace WebApp.Models
{
	public class PagosMayoresViewModel
	{
		public decimal Monto { get; set; }
		public bool IncludeUsers { get; set; }
		public IEnumerable<EquipoDTO> Equipos { get; set; } = Enumerable.Empty<EquipoDTO>();
	}

}
