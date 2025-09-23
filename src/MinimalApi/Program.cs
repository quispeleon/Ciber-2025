// ==============================
// DIRECTIVAS USING
// ==============================
using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Ciber.Dapper;
using Ciber.core;
using MinimalAPI.DTO;

// ==============================
// CONFIGURACIÓN Y REGISTRO DE SERVICIOS
// ==============================
var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySQL");

// Registrar IDbConnection para inyección de dependencias
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

// Registrar la interfaz IDAO y su implementación ADOD
builder.Services.AddScoped<IDAO, ADOD>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

// ==============================
// ENDPOINTS DE ALQUILERES
// ==============================

// Obtener todos los alquileres
app.MapGet("/alquileres", async (IDAO db) =>
{
    var alquileres = await db.ObtenerTodosLosAlquileresAsync();
    return Results.Ok(alquileres.Select(alquiler => new AlquilerDto(alquiler.Tipo, alquiler.CantidadTiempo, alquiler.Pagado)));
}).WithTags("Alquileres");

// Obtener alquiler por ID
app.MapGet("/alquileres/{id}", async (int id, IDAO db) =>
{
    var alquiler = await db.ObtenerAlquilerPorIdAsync(id);
    return alquiler is not null ? Results.Ok(alquiler) : Results.NotFound();
}).WithTags("Alquileres");

// Crear un nuevo alquiler
app.MapPost("/Alquileres", async (AlquilerAltaDTO alquiler, IDAO db) =>
{
    Alquiler alquilerAlta = new Alquiler
    {
        Ncuenta = alquiler.ncuenta,
        Nmaquina = alquiler.nmaquina,
        Tipo = alquiler.tipo,
        CantidadTiempo = alquiler.cantidadTiempo,
        Pagado = alquiler.Pagado
    };

    await db.AgregarAlquilerAsync(alquilerAlta, true);
    return Results.Created($"/alquileres/{alquilerAlta.IdAlquiler}", alquiler);
}).WithTags("Alquileres");


// ==============================
// ENDPOINTS DE MAQUINAS
// ==============================

// Obtener todas las maquinas
app.MapGet("/maquinas", async (IDAO db) =>
{
    var maquinas = await db.ObtenerTodasLasMaquinasAsync();
    return Results.Ok(maquinas.Select(maquina => new MaquinaDto(maquina.Estado, maquina.Caracteristicas)));
}).WithTags("Maquinas");

// Obtener máquina por ID
app.MapGet("/maquinas/{id}", async (int id, IDAO db) =>
{
    var maquina = await db.ObtenerMaquinaPorIdAsync(id);
    return maquina is not null ? Results.Ok(maquina) : Results.NotFound();
}).WithTags("Maquinas");

// Crear una nueva máquina
app.MapPost("/maquinas", async (MaquinaDto maquina, IDAO db) =>
{
    Maquina maquinaAlta = new Maquina
    {
        Estado = maquina.Estado,
        Caracteristicas = maquina.Caracteristicas
    };
    await db.AgregarMaquinaAsync(maquinaAlta);
    return Results.Created($"/maquinas/{maquinaAlta.Nmaquina}", maquina);
}).WithTags("Maquinas");

// ==============================
// ENDPOINTS DE CUENTAS
// ==============================

// Obtener todas las cuentas
app.MapGet("/cuentas", async (IDAO db) =>
{
    var cuentas = await db.ObtenerTodasLasCuentasAsync();
    return Results.Ok(cuentas.Select(cuenta => new CuentaDto(cuenta.Nombre, cuenta.Dni, cuenta.HoraRegistrada)));
}).WithTags("Cuentas");

// Obtener cuenta por ID
app.MapGet("/cuentas/{id}", async (int id, IDAO db) =>
{
    var cuenta = await db.ObtenerCuentaPorIdAsync(id);
    return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
}).WithTags("Cuentas");

// Crear una nueva cuenta
app.MapPost("/cuentas", async (CuentaAltaDTO cuenta, IDAO db) =>
{
    Cuenta cuentaAlta = new Cuenta
    {
        Nombre = cuenta.nombre,
        Pass = cuenta.pass,
        Dni = cuenta.dni,
        HoraRegistrada = cuenta.Horaregistrada
    };

    await db.AgregarCuentaAsync(cuentaAlta);
    return Results.Created($"/cuentas/{cuentaAlta.Ncuenta}", cuenta);
}).WithTags("Cuentas");

// ==============================
// ENDPOINTS DE MAQUINAS DISPONIBLES
// ==============================

// Obtener maquinas disponibles
app.MapGet("/maquinas/disponibles", async (IDAO db) =>
{
    var disponibles = await db.ObtenerMaquinaDisponiblesAsync();
    return Results.Ok(disponibles.Select(maquina => new MaquinaDto(maquina.Estado, maquina.Caracteristicas)));
}).WithTags("MaquinasDisponibles");

// Obtener maquinas no disponibles
app.MapGet("/maquinas/no-disponibles", async (IDAO db) =>
{
    var noDisponibles = await db.ObtenerMaquinaNoDisponiblesesAsync();
    return Results.Ok(noDisponibles.Select(maquina => new MaquinaDto(maquina.Estado, maquina.Caracteristicas)));
}).WithTags("MaquinasDisponibles");

// ==============================
// INICIAR LA APLICACIÓN
// ==============================
app.Run();
