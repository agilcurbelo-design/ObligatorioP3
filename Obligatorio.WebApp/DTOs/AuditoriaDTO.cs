namespace WebApp.DTOs
{
	public class AuditoriaDto
	{
		public DateTime Fecha { get; set; }
		public string Operacion { get; set; } = string.Empty;
		public string NombreUsuario { get; set; } = string.Empty;
		public string Detalles { get; set; } = string.Empty;
	}

}
