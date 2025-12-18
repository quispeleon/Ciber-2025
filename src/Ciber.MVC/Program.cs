using Ciber.Dapper;
using Ciber.core;
using System.Data;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =====================
// MVC
// =====================
builder.Services.AddControllersWithViews();

// =====================
// SESSION
// =====================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =====================
// CONEXIÓN MYSQL (DAPPER)
// =====================
var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(_ =>
    new MySqlConnection(connectionString));

builder.Services.AddScoped<IDAO, ADOD>();

// =====================
// AUTHENTICATION (COOKIES)
// =====================
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";              // NO autenticado
        options.AccessDeniedPath = "/Auth/AccesoDenegado"; // Autenticado sin permisos
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// =====================
// AUTHORIZATION (ROLES)
// =====================
builder.Services.AddAuthorization();

// =====================
// BUILD
// =====================
var app = builder.Build();

// =====================
// MIGRACIONES + ADMIN INICIAL
// =====================
using (var scope = app.Services.CreateScope())
{
    var dao = scope.ServiceProvider.GetRequiredService<IDAO>();

    await dao.MigrarUsuariosInsegurosAsync();

    var admin = await dao.ObtenerUsuarioSistemaPorUsernameAsync("admin");
    if (admin == null)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword("23456789");

        await dao.AgregarUsuarioSistemaAsync(new UsuarioSistema
        {
            Username = "admin",
            PasswordHash = passwordHash,
            Rol = "ADMIN_GENERAL",
            Activo = true
        });
    }
}

// =====================
// PIPELINE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();        // SIEMPRE antes de auth
app.UseAuthentication(); // 👈 OBLIGATORIO
app.UseAuthorization();  // 👈 OBLIGATORIO

// =====================
// ROUTING
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
