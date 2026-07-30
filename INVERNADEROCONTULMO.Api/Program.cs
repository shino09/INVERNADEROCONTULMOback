using System.Text;
using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Helpers;
using INVERNADEROCONTULMO.Api.Reports;
using INVERNADEROCONTULMO.Api.Services;

// Construcción de la aplicación
var builder = WebApplication.CreateBuilder(args);

// Configuración de controladores y Swagger
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración del proveedor de base de datos (Oracle o InMemory)
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "Oracle";
if (dbProvider == "InMemory")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("INVERNADERO")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));
}

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

// Registro de dependencias (servicios, helpers, reportes)
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IContabilidadService, ContabilidadService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Construcción de la aplicación web
var app = builder.Build();

// Middleware de manejo global de errores
app.Use(async (context, next) =>
{
    try { await next(); }
    catch (InvalidOperationException ex) { context.Response.StatusCode = 400; await context.Response.WriteAsJsonAsync(new { message = ex.Message }); }
    catch (Exception ex) { context.Response.StatusCode = 500; await context.Response.WriteAsJsonAsync(new { message = $"Error interno: {ex.Message}" }); }
});

// Middleware de Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "INVERNADEROCONTULMO API v1"));

// Middleware de CORS, autenticación, autorización y rutas
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Inicialización de la base de datos y siembra de datos iniciales
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        await ctx.Database.EnsureCreatedAsync();
        if (dbProvider == "InMemory") await DataSeeder.SeedAsync(ctx);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error conectando a la base de datos: {ex.Message}");
        Console.WriteLine("La aplicación se iniciará sin conexión a BD.");
        Console.WriteLine("Configure 'DatabaseProvider: InMemory' en appsettings.json para usar BD en memoria.");
    }
}

// Inicio de la aplicación
app.Run();
