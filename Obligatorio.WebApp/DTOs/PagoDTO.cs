namespace WebApp.DTOs
{

	public class PagoDTO
	{
		public int Id { get; set; }
		public decimal Monto { get; set; }
		public DateTime Fecha { get; set; }
		public String TipoGasto { get; set; }
	}

}
