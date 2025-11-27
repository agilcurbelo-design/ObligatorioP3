using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.DTOs;

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
		private string ObtenerToken() => HttpContext.Session.GetString("Token");

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
			{
				return RedirectToAction("Login", "Home");
			}
				return View("UsuarioConPago");
			
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


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> BuscarUsuarioID(int usuarioId)
		{
			var token = ObtenerToken();
			if (string.IsNullOrEmpty(token))
				return RedirectToAction("Login", "Home");

			try
			{
				var client = ObtenerClienteAutenticado();

				// Llamada al endpoint de la API que devuelve los pagos del usuario
				var response = await client.GetAsync($"api/pagos/usuario/{usuarioId}");

				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var pagos = JsonSerializer.Deserialize<List<PagoDTO>>(json, _jsonOptions) ?? new List<PagoDTO>();
					return View("UsuarioConPago", pagos);
				}

				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					return RedirectToAction("Login", "Home");

				// En caso de error, mostrar mensaje y lista vacía
				ViewBag.Error = $"Error al obtener pagos: {(int)response.StatusCode} {response.ReasonPhrase}";
				return View("UsuarioConPago", new List<PagoDTO>());
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error interno: {ex.Message}";
				return View("UsuarioConPago", new List<PagoDTO>());
			}
		}
		[HttpGet]
		public IActionResult CrearPago()
		{
			// Muestra la vista vacía; no usamos ViewModel, la vista construye el DTO por nombres de campos
			return View();
		}

		// POST: /Pago/CrearPago
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CrearPago([FromForm] CrearPagoDTO dto)
		{
			// Validación básica en servidor
			if (dto == null) return BadRequest("Datos inválidos.");
			if (!ModelState.IsValid) return View(dto); // si quieres mostrar errores en la vista, necesitarás adaptar la vista para mostrar ModelState

			var token = ObtenerToken();
			if (string.IsNullOrEmpty(token))
				return RedirectToAction("Login", "Home");

			try
			{
				var client = ObtenerClienteAutenticado();

				// Serializar DTO exactamente como espera la API
				var json = JsonSerializer.Serialize(dto, _jsonOptions);
				var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

				var response = await client.PostAsync("api/pagos", content);

				if (response.IsSuccessStatusCode)
				{
					// 201 Created: redirigir al listado o a detalle
					// opcional: leer el pago creado
					return RedirectToAction("Index");
				}

				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					return RedirectToAction("Login", "Home");

				// Leer mensaje de error devuelto por la API (si lo hay)
				var errorText = await response.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Error al crear pago: {(int)response.StatusCode} {response.ReasonPhrase} - {errorText}");
				return View(dto);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Error interno: {ex.Message}");
				return View(dto);
			}
		}


		// ViewModels para el MVC (puedes moverlos a Models si preferís)

	}
}
