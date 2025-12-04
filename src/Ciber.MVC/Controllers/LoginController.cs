using Microsoft.AspNetCore.Mvc;
using Ciber.core;

namespace Ciber.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDAO _dao;

        public LoginController(IDAO dao)
        {
            _dao = dao;
        }

        // POST: /Login/Autenticar
        [HttpPost]
        public async Task<IActionResult> Autenticar(string username, string password)
        {
            // VALIDAMOS USUARIO EN TU BASE
            var valido = await _dao.ValidarCredencialesAsync(username, password);

            if (!valido)
            {
                TempData["ErrorLogin"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Index", "Home");
            }

            // OBTENER CUENTA COMPLETA
            var cuenta = await _dao.ObtenerCuentaPorDniAsync(username);

            // GUARDAR EN SESIÓN
HttpContext.Session.SetString("UsuarioNombre", cuenta.Nombre);
HttpContext.Session.SetString("UsuarioDni", cuenta.Dni);    // con D minúscula
HttpContext.Session.SetInt32("UsuarioId", cuenta.Ncuenta);        // Id en vez de NCuenta

            return RedirectToAction("Index", "Home");
        }

        // GET: /Login/Cerrar
        public IActionResult Cerrar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
