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
builder.Services.AddScoped<IDAO>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MySQL");
    return new ADOD(connectionString);
});


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

// Cuentas
app.MapGet("/cuentas", (IDAO db) =>
{
    var cuentas = db.ObtenerTodasLasCuentas();
    return Results.Ok(cuentas);
});

app.MapGet("/cuentas/{id}", (int id, IDAO db) =>
{
    var cuenta = db.ObtenerCuentaPorId(id);
    return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
});

app.MapPost("/cuentas", (Cuenta cuenta, IDAO db) =>
{
    db.AgregarCuenta(cuenta);
    return Results.Created($"/cuentas/{cuenta.Ncuenta}", cuenta);
});

// maquinas
app.MapGet("/maquinas", (IDAO db) =>
{
    var maquinas = db.ObtenerTodasLasMaquinas();
    return Results.Ok(maquinas);
});

app.MapGet("/maquinas/{id}", (int id, IDAO db) =>
{
    var maquina = db.ObtenerMaquinaPorId(id);
    return maquina is not null ? Results.Ok(maquina) : Results.NotFound();
});

app.MapPost("/maquinas", (Maquina maquina, IDAO db) =>
{
    db.AgregarMaquina(maquina);
    return Results.Created($"/maquinas/{maquina.Nmaquina}", maquina);
});
app.Run();
