using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Ciber.Dapper;
using Ciber.core;


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


// cuentas

app.MapGet("/cuentas", async (IDAO db) =>
{
    var cuentas = await db.ObtenerTodasLasCuentasAsync();
    return Results.Ok(cuentas);
});

app.MapGet("/cuentas/{id}", async (int id, IDAO db) =>
{
    var cuenta = await db.ObtenerCuentaPorIdAsync(id);
    return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
});

app.MapPost("/cuentas", async (Cuenta cuenta, IDAO db) =>
{
    await db.AgregarCuentaAsync(cuenta);
    return Results.Created($"/cuentas/{cuenta.Ncuenta}", cuenta);
});

// maquinas

app.MapGet("/maquinas", async (IDAO db) =>
{
    var maquinas = await db.ObtenerTodasLasMaquinasAsync();
    return Results.Ok(maquinas);
});

app.MapGet("/maquinas/{id}", async (int id, IDAO db) =>
{
    var maquina = await db.ObtenerMaquinaPorIdAsync(id);
    return maquina is not null ? Results.Ok(maquina) : Results.NotFound();
});

app.MapPost("/maquinas", async (Maquina maquina, IDAO db) =>
{
    await db.AgregarMaquinaAsync(maquina);
    return Results.Created($"/maquinas/{maquina.Nmaquina}", maquina);
});

app.Run();