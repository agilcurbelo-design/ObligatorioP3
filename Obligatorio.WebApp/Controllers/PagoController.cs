using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Obligatorio.WebApp2.Controllers
{
	public class PagoController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		public PagoController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		// Helper: Obtener token de la sesión
		private string ObtenerToken() => HttpContext.Session.GetString("token");

		// Helper: Configurar HttpClient con token
		private HttpClient ObtenerClienteAutenticado()
		{
			var client = _httpClientFactory.CreateClient("api"); // Program.cs debe registrar "api" con BaseAddress apuntando a la API (idealmente incluye "api/")
			var token = ObtenerToken();

			if (!string.IsNullOrEmpty(token))
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return client;
		}

		// GET: /Pago  -> llama únicamente al endpoint que ya existe en la API:
		// GET api/pagos/usuario/{usuarioId}
		public async Task<IActionResult> Index()
		{
			var token = ObtenerToken();
			if (string.IsNullOrEmpty(token))
				return RedirectToAction("Login", "Home");

			try
			{
				var client = ObtenerClienteAutenticado();
				var usuarioId = HttpContext.Session.GetInt32("usuarioId");

				if (usuarioId == null)
				{
					ViewBag.Error = "No se encontró el usuario en sesión.";
					return View(new List<PagoViewModel>());
				}

				// Llamada CORRECTA a la API que ya tenés: "pagos" plural
				var response = await client.GetAsync($"pagos/usuario/{usuarioId}");

				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var pagos = JsonSerializer.Deserialize<List<PagoViewModel>>(json, _jsonOptions) ?? new List<PagoViewModel>();
					return View(pagos);
				}

				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					return RedirectToAction("Login", "Home");

				// devolver mensaje de error y lista vacía para la vista
				ViewBag.Error = $"Error al obtener pagos: {(int)response.StatusCode} {response.ReasonPhrase}";
				return View(new List<PagoViewModel>());
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error interno: {ex.Message}";
				return View(new List<PagoViewModel>());
			}
		}

		// NOTA: El backend que mostraste expone GET api/pagos/usuario/{usuarioId}.
		// Si tu API NO tiene GET api/pagos/{id}, NO llamamos a ese endpoint desde el frontend.
		// Aquí dejamos un stub informativo para Details que no hace llamadas inválidas.
		public IActionResult DetailsNotAvailable()
		{
			// Si en el futuro agregás GET api/pagos/{id} en el backend,
			// reemplazá este método por la versión que hace client.GetAsync($"pagos/{id}")
			return NotFound("El endpoint GET api/pagos/{id} no está implementado en la API. Añadilo en el backend para habilitar esta vista.");
		}

		// NOTA: Igual para creación: si la API expone POST api/pagos, podés activar el método Create.
		// Por ahora dejamos un stub para evitar llamadas a rutas inexistentes.
		public IActionResult CrearPagoNotAvailable()
		{
			return BadRequest("El endpoint POST api/pagos no está implementado en la API. Añadilo en el backend para habilitar la creación desde el frontend.");
		}
	}

	// ViewModels para el MVC (puedes moverlos a Models si preferís)
	public class PagoViewModel
	{
		public int Id { get; set; }
		public decimal Monto { get; set; }
		public DateTime Fecha { get; set; }
		public string Descripcion { get; set; }
		public string TipoGasto { get; set; }
		public string Usuario { get; set; }
	}
}
