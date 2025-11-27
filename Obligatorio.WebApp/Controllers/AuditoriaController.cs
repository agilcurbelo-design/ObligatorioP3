using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.DTOs;
using WebApp.Models;



public class AuditoriaController : Controller
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

	public AuditoriaController(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	private string ObtenerToken() => HttpContext.Session.GetString("Token") ?? string.Empty;

	private HttpClient ObtenerClienteAutenticado()
	{
		var client = _httpClientFactory.CreateClient("api");
		var token = ObtenerToken();
		if (!string.IsNullOrEmpty(token))
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		return client;
	}

	// GET: /Auditoria
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var vm = new PagosAuditoriaViewModel();

		// Cargar tipos de gasto para el select (si existe endpoint)
		try
		{
			var client = ObtenerClienteAutenticado();
			var resp = await client.GetAsync("api/TipoGasto"); // ajustá la ruta si tu API la expone distinto
			if (resp.IsSuccessStatusCode)
			{
				var json = await resp.Content.ReadAsStringAsync();
				vm.TiposDeGasto = JsonSerializer.Deserialize<List<TipoGastoDTO>>(json, _jsonOptions) ?? new List<TipoGastoDTO>();
			}
		}
		catch
		{
			// no bloqueamos la vista si falla la carga de tipos; se mostrará vacío
		}

		return View(vm);
	}

	// POST: /Auditoria (filtrar por tipo de gasto)
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Index(PagosAuditoriaViewModel vm)
	{
		// Volver a cargar tipos de gasto para el select
		try
		{
			var client = ObtenerClienteAutenticado();
			var respTipos = await client.GetAsync("api/TipoGasto");
			if (respTipos.IsSuccessStatusCode)
			{
				var jsonTipos = await respTipos.Content.ReadAsStringAsync();
				vm.TiposDeGasto = JsonSerializer.Deserialize<List<TipoGastoDTO>>(jsonTipos, _jsonOptions) ?? new List<TipoGastoDTO>();
			}
		}
		catch
		{
			vm.TiposDeGasto = vm.TiposDeGasto ?? new List<TipoGastoDTO>();
		}

		if (!vm.TipoGastoId.HasValue)
		{
			ModelState.AddModelError(nameof(vm.TipoGastoId), "Seleccione un tipo de gasto.");
			return View(vm);
		}

		var token = ObtenerToken();
		if (string.IsNullOrEmpty(token))
			return RedirectToAction("Login", "Home");

		try
		{
			var client = ObtenerClienteAutenticado();
			var url = $"api/Auditoria/tipoGasto/{vm.TipoGastoId.Value}";
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
			var resultados = JsonSerializer.Deserialize<List<AuditoriaDto>>(json, _jsonOptions) ?? new List<AuditoriaDto>();

			// Aseguramos orden descendente por fecha
			vm.Auditorias = resultados.OrderByDescending(a => a.Fecha).ToList();

			return View(vm);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, $"Error interno: {ex.Message}");
			return View(vm);
		}
	}
}
