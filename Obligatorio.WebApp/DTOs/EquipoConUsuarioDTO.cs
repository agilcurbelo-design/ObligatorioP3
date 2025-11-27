namespace WebApp.DTOs
{
	public class EquipoConUsuarioDTO : EquipoDTO
	{
		public List<UsuarioDTO> Usuarios { get; set; } = new();
	}

}
