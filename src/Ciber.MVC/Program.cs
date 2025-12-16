using Ciber.Dapper;
using Ciber.core;
using System.Data;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =====================
// MVC + SESSION
// =====================
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// =====================
// CONEXIÓN A MYSQL (DAPPER)
// =====================
var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(sp =>
{
    return new MySqlConnection(connectionString);
});

builder.Services.AddScoped<IDAO, ADOD>();

// =====================
// AUTENTICACIÓN (COOKIES)
// =====================
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// =====================
// AUTORIZACIÓN (ROLES)
// =====================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN_GENERAL", policy =>
        policy.RequireRole("ADMIN_GENERAL"));

    options.AddPolicy("ADMIN_FINANZAS", policy =>
        policy.RequireRole("ADMIN_FINANZAS"));
});

var app = builder.Build();

// =====================
// MIGRAR USUARIOS Y CREAR ADMIN INICIAL
// =====================
using (var scope = app.Services.CreateScope())
{
    var dao = scope.ServiceProvider.GetRequiredService<IDAO>();

    // Migrar usuarios inseguros (contraseñas en texto plano)
    await dao.MigrarUsuariosInsegurosAsync();

    // Crear usuario admin si no existe
    var adminExistente = await dao.ObtenerUsuarioSistemaPorUsernameAsync("admin");
    if (adminExistente == null)
    {
        string password = "23456789"; // mínimo 8 caracteres
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        await dao.AgregarUsuarioSistemaAsync(new UsuarioSistema
        {
            Username = "admin",
            PasswordHash = passwordHash,
            Rol = "ADMIN_GENERAL",
            Activo = true
        });

        Console.WriteLine("Usuario admin creado con contraseña segura.");
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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// =====================
// RUTEO
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
