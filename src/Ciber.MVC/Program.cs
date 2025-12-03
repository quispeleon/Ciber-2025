using Ciber.Dapper;
using Ciber.core;
using System.Data;
using MySqlConnector;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();



//  Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySQL");

//  Registrando IDbConnection para que se inyecte como dependencia
//  Cada vez que se inyecte, se creará una nueva instancia con la cadena de conexión
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
builder.Services.AddScoped<IDAO, ADOD>(); // conexion pe



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

var username = "brenda"; // Cambia a tu usuario
var password = "brenda123";  // Cambia a tu contraseña

app.UseMiddleware<BasicAuthMiddleware>(username, password);

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
