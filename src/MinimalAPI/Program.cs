using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Ciber.Dapper;
using Ciber.core;
using MinimalAPI.DTO;


var builder = WebApplication.CreateBuilder(args);

//  Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySQL");

//  Registrando IDbConnection para que se inyecte como dependencia
//  Cada vez que se inyecte, se creará una nueva instancia con la cadena de conexión
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

//Cada vez que necesite la interfaz, se va a instanciar automaticamente AdoDapper y se va a pasar al metodo de la API
builder.Services.AddScoped<IDAO, ADOD>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}


// ALQUILERES

app.MapGet("/alquileres", async (IDAO db) =>
{
    var alquileres = await db.ObtenerTodosLosAlquileresAsync();
    return Results.Ok(alquileres.Select(alquileres => new AlquilerDto(alquileres.Tipo, alquileres.CantidadTiempo, alquileres.Pagado )));
}).WithTags("Alquileres");

app.MapGet("/alquileres/{id}", async (int id, IDAO db) =>
{
    var alquiler = await db.ObtenerAlquilerPorIdAsync(id);
    return alquiler is not null ? Results.Ok(alquiler) : Results.NotFound();
}).WithTags("Alquileres");

app.MapPost("/Alquileres", async (AlquilerAltaDTO alquiler, IDAO db) =>
{

    Alquiler alquileralta = new Alquiler
    {
        Ncuenta = alquiler.ncuenta,
        Nmaquina = alquiler.nmaquina,
        Tipo = alquiler.tipo,
        CantidadTiempo = alquiler.cantidadTiempo,
        Pagado = alquiler.Pagado
    };

    await db.AgregarAlquilerAsync(alquileralta, true);
    return Results.Created($"/alquileres/{alquileralta.IdAlquiler}", alquiler);
}).WithTags("Alquileres");



// maquinas

app.MapGet("/maquinas", async (IDAO db) =>
{
    var maquinas = await db.ObtenerTodasLasMaquinasAsync();
    return Results.Ok(maquinas.Select(maquina => new MaquinaDto(maquina.Estado, maquina.Caracteristicas)));
}).WithTags("maquinas");

app.MapGet("/maquinas/{id}", async (int id, IDAO db) =>
{
    var maquina = await db.ObtenerMaquinaPorIdAsync(id);
    return maquina is not null ? Results.Ok(maquina) : Results.NotFound();
}).WithTags("maquinas");

app.MapPost("/maquinas", async (MaquinaDto maquina, IDAO db) =>
{
    Maquina maquinaAlta = new Maquina
    {
        Estado = maquina.Estado,
        Caracteristicas = maquina.Caracteristicas
    };
    await db.AgregarMaquinaAsync(maquinaAlta);
    return Results.Created($"/maquinas/{maquinaAlta.Nmaquina}", maquina);
}).WithTags("maquinas");

// Cuentas

app.MapGet("/cuentas", async (IDAO db) =>
{
    var cuentas = await db.ObtenerTodasLasCuentasAsync();
    return Results.Ok(cuentas.Select(cuenta => new CuentaDto(cuenta.Nombre, cuenta.Dni, cuenta.HoraRegistrada )) );
}).WithTags("cuentas");

app.MapGet("/cuentas/{id}", async (int id, IDAO db) =>
{
    var cuenta = await db.ObtenerCuentaPorIdAsync(id);
    return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
}).WithTags("cuentas");

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
}).WithTags("cuentas");

app.MapGet("/maquinas/disponibles", async (IDAO db) =>
{
    var disponibles = await db.ObtenerMaquinaDisponiblesAsync();
    return Results.Ok(disponibles.Select(disponibles => new MaquinaDto(disponibles.Estado , disponibles.Caracteristicas)));
}).WithTags("MaquinasDisponibles");



app.MapGet("/maquinas/no-disponibles", async (IDAO db) =>
{
    var noDisponibles = await db.ObtenerMaquinaNoDisponiblesesAsync();
    return Results.Ok(noDisponibles.Select(noDisponibles=> new MaquinaDto(noDisponibles.Estado ,noDisponibles.Caracteristicas)));
}).WithTags("MaquinasDisponibles");


app.Run();