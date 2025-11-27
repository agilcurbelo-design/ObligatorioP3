using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ZONA DE SERVICIOS
// ==========================================

builder.Services.AddControllersWithViews();

// A. Configurar Cliente HTTP (El teléfono rojo hacia la API)
builder.Services.AddHttpClient("api", client =>
{
	// <--- PON AQUÍ LA URL DE TU BACKEND (API)
	client.BaseAddress = new Uri("http://localhost:5163/");
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// B. Configurar Sesión (La billetera donde guardas el Token)
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60); // El token vivirá en memoria 60 mins
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// C. Accesor de contexto (Para poder leer la sesión desde clases que no sean Controllers)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ==========================================
// 2. PIPELINE DE SOLICITUDES
// ==========================================

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 1. Activar Sesión (Obligatorio antes de intentar leer/escribir el token)
app.UseSession();

// 2. Autorización (Manejo de rutas protegidas en MVC)
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=home}/{action=Login}/{id?}"); // Arranca en el Login

app.Run();