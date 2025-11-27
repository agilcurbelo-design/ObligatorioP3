using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.DTOs;
using WebApp.Models;



public class EquipoController : Controller
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

	public EquipoController(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	private string ObtenerToken() => HttpContext.Session.GetString("Token") ?? string.Empty;

	private HttpClient ObtenerClienteAutenticado()
	{
		var client = _httpClientFactory.CreateClient("api");
		var token = ObtenerToken();
		if (!string.IsNullOrEmpty(token))
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
		return client;
	}

	[HttpGet]
	public IActionResult PagosMayores()
	{
		return View(new PagosMayoresViewModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> PagosMayores(PagosMayoresViewModel vm)
	{
		if (vm == null) return BadRequest();

		// Validación RF4: monto no negativo y mayor a 0 (según tu regla de negocio)
		if (vm.Monto <= 0)
		{
			ModelState.AddModelError(nameof(vm.Monto), "El monto debe ser mayor a 0.");
			return View(vm);
		}

		var token = ObtenerToken();
		if (string.IsNullOrEmpty(token))
			return RedirectToAction("Login", "Home");

		try
		{
			var client = ObtenerClienteAutenticado();
			var url = $"api/Equipos/pagos-mayores/{vm.Monto}"; // sin includeUsers

			var response = await client.GetAsync(url);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				return RedirectToAction("Login", "Home");

			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Error al consultar la API: {(int)response.StatusCode} {response.ReasonPhrase} - {error}");
				return View(vm);
			}

			var json = await response.Content.ReadAsStringAsync();
			vm.Equipos = JsonSerializer.Deserialize<List<EquipoDTO>>(json, _jsonOptions) ?? new List<EquipoDTO>();

			// Aseguramos orden descendente por nombre en el frontend por si la API no lo hizo
			vm.Equipos = vm.Equipos.OrderByDescending(e => e.Nombre).ToList();

			return View(vm);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Error interno: {ex.Message}");
			return View(vm);
		}
	}
}
