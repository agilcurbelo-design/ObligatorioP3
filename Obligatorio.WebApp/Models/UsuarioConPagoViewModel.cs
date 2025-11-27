using WebApp.DTOs;

namespace WebApp.Models
{
	public class UsuarioConPagosViewModel
	{
		public UsuarioDTO Usuario { get; set; }
		public IEnumerable<PagoDTO> Pagos { get; set; } = new List<PagoDTO>();
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public int TotalItems { get; set; } = 0;
	}

}
