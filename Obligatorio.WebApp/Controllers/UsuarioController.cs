using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApp.Models; // donde pondrás el ViewModel
using WebApp.DTOs; // si ya tienes DTOs compartidos
using Microsoft.AspNetCore.Http;

namespace TuProyectoMVC.Controllers
{
	public class UsuarioController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		public UsuarioController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				var client = _httpClientFactory.CreateClient("ObligatorioApi");

				var loginDto = new { email = model.Email, contrasenia = model.Password };
				var json = JsonSerializer.Serialize(loginDto);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await client.PostAsync("auth/login", content);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<LoginResponse>(jsonResponse, _jsonOptions);

					HttpContext.Session.SetString("token", result.Token);
					HttpContext.Session.SetString("usuario", result.Usuario.Nombre);
					HttpContext.Session.SetInt32("usuarioId", result.Usuario.Id);
					HttpContext.Session.SetString("rol", result.Usuario.Rol);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ViewBag.Error = "Credenciales inválidas";
					return View(model);
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error: {ex.Message}";
				return View(model);
			}
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}

		// GET: /Usuario/Detalle/5?page=1&pageSize=20
		[HttpGet]
		public async Task<IActionResult> Detalle(int id, int page = 1, int pageSize = 20)
		{
			var vm = new UsuarioConPagosViewModel
			{
				Page = page,
				PageSize = pageSize
			};

			var token = HttpContext.Session.GetString("token"); // usar la misma clave que en Login
			var client = _httpClientFactory.CreateClient("ObligatorioApi"); // mismo nombre que en Login

			if (!string.IsNullOrEmpty(token))
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// 1) Obtener usuario (opcional)
			try
			{
				var userResp = await client.GetAsync($"api/usuarios/{id}");
				if (userResp.IsSuccessStatusCode)
				{
					var userJson = await userResp.Content.ReadAsStringAsync();
					vm.Usuario = JsonSerializer.Deserialize<UsuarioDTO>(userJson, _jsonOptions);
				}
				else if (userResp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					return RedirectToAction("Login");
				}
			}
			catch
			{
				// opcional: log
			}

			// 2) Obtener pagos (paginados si la API lo soporta)
			try
			{
				var pagosResp = await client.GetAsync($"api/pagos/usuario/{id}?page={page}&pageSize={pageSize}");
				if (pagosResp.IsSuccessStatusCode)
				{
					var pagosJson = await pagosResp.Content.ReadAsStringAsync();
					vm.Pagos = JsonSerializer.Deserialize<IEnumerable<PagoDTO>>(pagosJson, _jsonOptions) ?? new List<PagoDTO>();

					if (pagosResp.Headers.Contains("X-Total-Count"))
					{
						if (int.TryParse(pagosResp.Headers.GetValues("X-Total-Count").FirstOrDefault(), out var total))
							vm.TotalItems = total;
					}
				}
				else if (pagosResp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					return RedirectToAction("Login");
				}
				else if (pagosResp.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					return Forbid();
				}
			}
			catch
			{
				// opcional: log
			}

			return View(vm);
		}
	}

	// Clases auxiliares para Login (pueden quedar aquí o en archivos separados)
	public class LoginViewModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class LoginResponse
	{
		public string Token { get; set; }
		public UsuarioInfo Usuario { get; set; }
	}

	public class UsuarioInfo
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string Email { get; set; }
		public string Rol { get; set; }
	}
}
