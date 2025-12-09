using Ciber.Dapper;
using Ciber.core;
using System.Data;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Conexión
var connectionString = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
builder.Services.AddScoped<IDAO, ADOD>();

// 🔥 AUTENTICACIÓN POR COOKIES (OBLIGATORIO PARA LOGIN)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // cuando intenta entrar a algo protegido
        options.AccessDeniedPath = "/Login"; 
    });

// AUTORIZACIÓN
builder.Services.AddAuthorization();

var app = builder.Build();

// PIPELINE -----------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// 🔥 EL ORDEN CORRECTO
app.UseAuthentication(); // primero
app.UseAuthorization();  // después

app.MapStaticAssets();
app.UseSession();

// RUTEO
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

