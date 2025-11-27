using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using WebApp.DTOs;
using System.Net.Http; 
					   // Asegúrate de inyectar HttpClient en el constructor
public class HomeController : Controller
{
	private readonly HttpClient _httpClient;


	// ¡CAMBIO CLAVE AQUÍ! Inyectar la fábrica.
	public HomeController(IHttpClientFactory httpClientFactory)
	{
		// Obtener el cliente nombrado "Api" que definiste en Program.cs
		_httpClient = httpClientFactory.CreateClient("api");
		// ELIMINAR: _httpClient.BaseAddress = new Uri(ApiBaseUrl);
	}

	public IActionResult Index()
	{
		// Si hay token, mostrar la Home Page real, sino, redirigir a Login
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
		{
			return RedirectToAction("Login");
		}
		return View();
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View(); // Muestra el formulario de login (una vista con un formulario que usa LoginDTO)
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginDTO loginDto)
	{
		if (!ModelState.IsValid)
		{
			return View(loginDto);
		}

		try
		{
			// 1. Preparar la solicitud HTTP para la API
			var jsonContent = JsonSerializer.Serialize(loginDto);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			// 2. Llamar al endpoint de Login de la Web API
			var response = await _httpClient.PostAsync("api/Auth/login", content);

			if (response.IsSuccessStatusCode)
			{
				var apiResponseContent = await response.Content.ReadAsStringAsync();
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDTO>(
					apiResponseContent,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
				);

				// 3. Almacenar el Token y los datos del usuario en la Sesión
				HttpContext.Session.SetString("Token", loginResponse.Token);
				HttpContext.Session.SetString("Rol", loginResponse.Usuario.Rol);
				HttpContext.Session.SetInt32("UsuarioId", loginResponse.Usuario.Id);

				TempData["Mensaje"] = $"Bienvenido/a {loginResponse.Usuario.Nombre}. Rol: {loginResponse.Usuario.Rol}";
				return RedirectToAction("Index", "Home");
			}
			else
			{
				// Manejo de errores 401/400
				ViewBag.Error = "Error al iniciar sesión. Verifique sus credenciales.";
				return View(loginDto);
			}
		}
		catch (Exception ex)
		{
			ViewBag.Error = $"Error de conexión con la API: {ex.Message}";
			return View(loginDto);
		}
	}

	public IActionResult Logout()
	{
		HttpContext.Session.Clear(); // Elimina todos los datos de la sesión (incluyendo el token)
		TempData["Mensaje"] = "Sesión cerrada correctamente.";
		return RedirectToAction("Login", "Home");
	}

}