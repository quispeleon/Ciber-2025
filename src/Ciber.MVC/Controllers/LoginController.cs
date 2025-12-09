using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Ciber.core;

public class LoginController : Controller
{
    private readonly IDAO _dao;

    public LoginController(IDAO dao)
    {
        _dao = dao;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(); // Vista de login
    }

    [HttpPost]
    public async Task<IActionResult> Index(string dni, string password)
    {
        if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Ingrese DNI y contraseña.";
            return View();
        }

        // **1) Validar credenciales**
        bool valido = await _dao.ValidarCredencialesAsync(dni, password);

        if (!valido)
        {
            ViewBag.Error = "DNI o contraseña incorrectos.";
            return View();
        }

        // **2) Obtener la cuenta**
        var cuenta = await _dao.ObtenerCuentaPorDniAsync(dni);

        if (cuenta == null)
        {
            ViewBag.Error = "Cuenta no encontrada.";
            return View();
        }

        // **3) Crear claims**
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, cuenta.Ncuenta.ToString()),
            new Claim(ClaimTypes.Name, cuenta.Nombre),
            new Claim("Dni", cuenta.Dni),
            new Claim("Saldo", cuenta.Saldo.ToString()),
            new Claim("Activa", cuenta.Activa.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var principal = new ClaimsPrincipal(identity);

        // **4) Iniciar sesión**
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }
}
