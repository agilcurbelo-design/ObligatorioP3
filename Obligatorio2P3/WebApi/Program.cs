using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Obligatorio.AccesoDatos;
using Obligatorio.AccesoDatos.Repositorios;
using Obligatorio.LogicaDeAplicacion.CasosDeUso.Usuarios;
using Obligatorio.LogicaDeAplicacion.CasosDeUso.Auditoria;
using Obligatorio.LogicaDeAplicacion.InterfacesDeCasoDeUso;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.LogicaNegocio.InterfacesRepositorio;
using Obligatorio.WebApi.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS: permitir solo la WebApp en desarrollo
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowWebApp", policy =>
	{
		policy.WithOrigins("https://localhost:7108")
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});
});

// Controllers + JSON options
builder.Services.AddControllers()
	.AddJsonOptions(opts =>
	{
		opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
	});

builder.Services.AddEndpointsApiExplorer();

// Swagger / OpenAPI
var rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Obligatorio.WebApp2.xml");
builder.Services.AddSwaggerGen(opciones =>
{
	opciones.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
	{
		Description = "Autorización estándar mediante esquema Bearer",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});

	opciones.OperationFilter<SecurityRequirementsOperationFilter>();

	opciones.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Documentación de Obligatorio.Api",
		Description = "Aqui se encuentran todos los endpoint activos para utilizar los servicios del Obligatorio de Agustin Gil",
		Contact = new OpenApiContact { Email = "aguatingil@gmail.com" },
		Version = "v1"
	});

	// Incluir comentarios XML si existe el archivo
	if (System.IO.File.Exists(rutaArchivo))
	{
		opciones.IncludeXmlComments(rutaArchivo);
	}
});

// Session (parece que la app usa Session)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.IdleTimeout = TimeSpan.FromHours(2);
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
{
	opciones.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? string.Empty)),
		ValidateIssuer = false,
		ValidateAudience = false,
	};
});

// DbContext
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OblContext>(options =>
{
	options.UseSqlServer(connectionString)
		   .EnableDetailedErrors();
});

// Repositorios y casos de uso
builder.Services.AddScoped<IRepositorioUsuario, SQLRepositorioUsuario>();
builder.Services.AddScoped<IRepositorioPago, SQLRepositorioPago>();
builder.Services.AddScoped<IRepositorioTipoGasto, SQLRepositorioTipoGasto>();
builder.Services.AddScoped<ILogin, LoginCU>();
builder.Services.AddScoped<IServicioPagoCU, ServicioPagoCU>();
builder.Services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
builder.Services.AddScoped<IRepositorioAuditoria, SQLRepositorioAuditoria>();

// Registro de IAuditoriaCU: ajustá la implementación concreta si tu clase tiene otro nombre o namespace
// Ejemplo: builder.Services.AddScoped<IAuditoriaCU, AuditoriaCU>();
builder.Services.AddScoped<IAuditoriaCU, AuditoriaCU>();

// Agregá aquí otros repositorios o casos de uso que uses
// builder.Services.AddScoped<IRepositorioOtro, SQLRepositorioOtro>();

// Authorization
builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tu API v1");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();

app.UseRouting();

// Aplicar CORS antes de auth si querés permitir llamadas desde la webapp
app.UseCors("AllowWebApp");

// Session debe ir antes de Authentication/Authorization si la usás para almacenar datos de sesión
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
